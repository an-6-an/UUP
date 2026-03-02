using Microsoft.AspNetCore.Mvc;
using AutoService.Data;
using AutoService.Models;

namespace AutoService.Controllers;

public class ImportController : Controller
{
    private readonly AppDbContext _db;

    public ImportController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsAuthorized() => HttpContext.Session.GetString("UserLogin") != null;
    private string GetUserType() => HttpContext.Session.GetString("UserType") ?? "";

    public IActionResult Index()
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (GetUserType() != "Менеджер" && GetUserType() != "Оператор") return Forbid();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImportUsers(IFormFile file)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (GetUserType() != "Менеджер") return Forbid();

        if (file == null || file.Length == 0)
        {
            ViewBag.Error = "Файл не выбран.";
            return View("Index");
        }

        int count = 0;
        using var reader = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8);
        string? line;
        bool firstLine = true;
        var errors = new List<string>();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (firstLine) { firstLine = false; continue; } // skip header
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(';');
            if (parts.Length < 6) { errors.Add($"Строка пропущена (мало полей): {line}"); continue; }

            try
            {
                var user = new User
                {
                    UserID = int.Parse(parts[0].Trim()),
                    FIO = parts[1].Trim(),
                    Phone = parts[2].Trim(),
                    Login = parts[3].Trim(),
                    Password = parts[4].Trim(),
                    Type = parts[5].Trim()
                };

                var existing = await _db.Users.FindAsync(user.UserID);
                if (existing != null)
                {
                    existing.FIO = user.FIO;
                    existing.Phone = user.Phone;
                    existing.Login = user.Login;
                    existing.Password = user.Password;
                    existing.Type = user.Type;
                }
                else
                {
                    _db.Users.Add(user);
                }
                count++;
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка в строке: {line} — {ex.Message}");
            }
        }

        await _db.SaveChangesAsync();
        ViewBag.Success = $"Импортировано пользователей: {count}";
        ViewBag.Errors = errors;
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ImportRequests(IFormFile file)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (GetUserType() != "Менеджер" && GetUserType() != "Оператор") return Forbid();

        if (file == null || file.Length == 0)
        {
            ViewBag.Error = "Файл не выбран.";
            return View("Index");
        }

        int count = 0;
        using var reader = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8);
        string? line;
        bool firstLine = true;
        var errors = new List<string>();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (firstLine) { firstLine = false; continue; }
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(';');
            if (parts.Length < 10) { errors.Add($"Строка пропущена (мало полей): {line}"); continue; }

            try
            {
                var request = new Request
                {
                    RequestID = int.Parse(parts[0].Trim()),
                    StartDate = DateTime.Parse(parts[1].Trim()),
                    CarType = parts[2].Trim(),
                    CarModel = parts[3].Trim(),
                    ProblemDescription = parts[4].Trim(),
                    RequestStatus = parts[5].Trim(),
                    CompletionDate = parts[6].Trim() == "null" || string.IsNullOrEmpty(parts[6].Trim()) ? null : DateTime.Parse(parts[6].Trim()),
                    RepairParts = string.IsNullOrEmpty(parts[7].Trim()) ? null : parts[7].Trim(),
                    MasterID = parts[8].Trim() == "null" || string.IsNullOrEmpty(parts[8].Trim()) ? null : int.Parse(parts[8].Trim()),
                    ClientID = int.Parse(parts[9].Trim())
                };

                var existing = await _db.Requests.FindAsync(request.RequestID);
                if (existing != null)
                {
                    existing.StartDate = request.StartDate;
                    existing.CarType = request.CarType;
                    existing.CarModel = request.CarModel;
                    existing.ProblemDescription = request.ProblemDescription;
                    existing.RequestStatus = request.RequestStatus;
                    existing.CompletionDate = request.CompletionDate;
                    existing.RepairParts = request.RepairParts;
                    existing.MasterID = request.MasterID;
                    existing.ClientID = request.ClientID;
                }
                else
                {
                    _db.Requests.Add(request);
                }
                count++;
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка в строке: {line} — {ex.Message}");
            }
        }

        await _db.SaveChangesAsync();
        ViewBag.Success = $"Импортировано заявок: {count}";
        ViewBag.Errors = errors;
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ImportComments(IFormFile file)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (GetUserType() != "Менеджер" && GetUserType() != "Оператор") return Forbid();

        if (file == null || file.Length == 0)
        {
            ViewBag.Error = "Файл не выбран.";
            return View("Index");
        }

        int count = 0;
        using var reader = new StreamReader(file.OpenReadStream(), System.Text.Encoding.UTF8);
        string? line;
        bool firstLine = true;
        var errors = new List<string>();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (firstLine) { firstLine = false; continue; }
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(';');
            if (parts.Length < 4) { errors.Add($"Строка пропущена: {line}"); continue; }

            try
            {
                var comment = new Comment
                {
                    CommentID = int.Parse(parts[0].Trim()),
                    Message = parts[1].Trim(),
                    MasterID = int.Parse(parts[2].Trim()),
                    RequestID = int.Parse(parts[3].Trim())
                };

                var existing = await _db.Comments.FindAsync(comment.CommentID);
                if (existing == null)
                    _db.Comments.Add(comment);
                count++;
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка в строке: {line} — {ex.Message}");
            }
        }

        await _db.SaveChangesAsync();
        ViewBag.Success = $"Импортировано комментариев: {count}";
        ViewBag.Errors = errors;
        return View("Index");
    }
}
