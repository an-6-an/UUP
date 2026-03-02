using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;

namespace AutoService.Controllers;

public class RequestsController : Controller
{
    private readonly AppDbContext _db;

    public RequestsController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsAuthorized() => HttpContext.Session.GetString("UserLogin") != null;
    private int GetUserID() => HttpContext.Session.GetInt32("UserID") ?? 0;
    private string GetUserType() => HttpContext.Session.GetString("UserType") ?? "";

    public async Task<IActionResult> Index(string? search, string? status)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var query = _db.Requests
            .Include(r => r.Client)
            .Include(r => r.Master)
            .AsQueryable();

        var userType = GetUserType();
        var userId = GetUserID();

        // Заказчик видит только свои заявки
        if (userType == "Заказчик")
            query = query.Where(r => r.ClientID == userId);

        // Автомеханик видит назначенные ему заявки
        if (userType == "Автомеханик")
            query = query.Where(r => r.MasterID == userId);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(r => r.RequestID.ToString().Contains(search) ||
                                     r.CarModel.Contains(search) ||
                                     r.Client.FIO.Contains(search));

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.RequestStatus == status);

        ViewBag.Search = search;
        ViewBag.Status = status;
        ViewBag.UserType = userType;
        return View(await query.OrderByDescending(r => r.StartDate).ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var request = await _db.Requests
            .Include(r => r.Client)
            .Include(r => r.Master)
            .Include(r => r.Comments).ThenInclude(c => c.Master)
            .FirstOrDefaultAsync(r => r.RequestID == id);

        if (request == null) return NotFound();

        var userType = GetUserType();
        var userId = GetUserID();

        // Заказчик может видеть только свои заявки
        if (userType == "Заказчик" && request.ClientID != userId)
            return Forbid();

        ViewBag.UserType = userType;
        ViewBag.UserID = userId;
        ViewBag.Mechanics = await _db.Users.Where(u => u.Type == "Автомеханик").ToListAsync();
        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        var userType = GetUserType();
        if (userType != "Заказчик" && userType != "Оператор" && userType != "Менеджер")
            return Forbid();
        ViewBag.Clients = await _db.Users.Where(u => u.Type == "Заказчик").ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Request request)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var userType = GetUserType();
        var userId = GetUserID();

        request.StartDate = DateTime.Now;
        request.RequestStatus = "Новая заявка";

        if (userType == "Заказчик")
            request.ClientID = userId;

        _db.Requests.Add(request);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var request = await _db.Requests.Include(r => r.Client).FirstOrDefaultAsync(r => r.RequestID == id);
        if (request == null) return NotFound();

        var userType = GetUserType();
        var userId = GetUserID();

        // Заказчик может редактировать только свои заявки в статусе "Новая заявка"
        if (userType == "Заказчик" && (request.ClientID != userId || request.RequestStatus != "Новая заявка"))
            return Forbid();

        if (userType == "Автомеханик" && request.MasterID != userId)
            return Forbid();

        ViewBag.UserType = userType;
        ViewBag.Mechanics = await _db.Users.Where(u => u.Type == "Автомеханик").ToListAsync();
        ViewBag.Clients = await _db.Users.Where(u => u.Type == "Заказчик").ToListAsync();
        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Request request)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var existing = await _db.Requests.FindAsync(request.RequestID);
        if (existing == null) return NotFound();

        var userType = GetUserType();
        var userId = GetUserID();

        if (userType == "Заказчик")
        {
            // Заказчик может менять только описание проблемы
            existing.ProblemDescription = request.ProblemDescription;
            existing.CarType = request.CarType;
            existing.CarModel = request.CarModel;
        }
        else if (userType == "Автомеханик")
        {
            existing.RequestStatus = request.RequestStatus;
            existing.RepairParts = request.RepairParts;
            if (request.RequestStatus == "Готова к выдаче" || request.RequestStatus == "Завершена")
                existing.CompletionDate = DateTime.Now;
        }
        else // Оператор, Менеджер
        {
            existing.CarType = request.CarType;
            existing.CarModel = request.CarModel;
            existing.ProblemDescription = request.ProblemDescription;
            existing.RequestStatus = request.RequestStatus;
            existing.MasterID = request.MasterID;
            existing.RepairParts = request.RepairParts;
            existing.ClientID = request.ClientID;
            if (request.RequestStatus == "Готова к выдаче" || request.RequestStatus == "Завершена")
                existing.CompletionDate = existing.CompletionDate ?? DateTime.Now;
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var userType = GetUserType();
        if (userType != "Менеджер" && userType != "Оператор") return Forbid();

        var request = await _db.Requests.FindAsync(id);
        if (request != null)
        {
            _db.Requests.Remove(request);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int requestId, string message)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");

        var userType = GetUserType();
        var userId = GetUserID();

        if (userType != "Автомеханик" && userType != "Менеджер")
            return Forbid();

        var comment = new Comment
        {
            Message = message,
            MasterID = userId,
            RequestID = requestId
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", new { id = requestId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteComment(int commentId, int requestId)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        var userType = GetUserType();
        if (userType != "Менеджер") return Forbid();

        var comment = await _db.Comments.FindAsync(commentId);
        if (comment != null)
        {
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Details", new { id = requestId });
    }
}
