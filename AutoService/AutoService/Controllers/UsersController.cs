using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;

namespace AutoService.Controllers;

public class UsersController : Controller
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    private bool IsManager() => HttpContext.Session.GetString("UserType") == "Менеджер";
    private bool IsAuthorized() => HttpContext.Session.GetString("UserLogin") != null;

    public async Task<IActionResult> Index()
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();

        var users = await _db.Users.ToListAsync();
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();

        if (await _db.Users.AnyAsync(u => u.Login == user.Login))
        {
            ModelState.AddModelError("Login", "Пользователь с таким логином уже существует.");
            return View(user);
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();

        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();

        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAuthorized()) return RedirectToAction("Login", "Account");
        if (!IsManager()) return Forbid();

        var user = await _db.Users.FindAsync(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
