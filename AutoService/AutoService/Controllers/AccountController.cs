using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;

namespace AutoService.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserLogin") != null)
            return RedirectToAction("Index", "Requests");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string login, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        if (user == null)
        {
            ViewBag.Error = "Неверный логин или пароль.";
            return View();
        }

        HttpContext.Session.SetInt32("UserID", user.UserID);
        HttpContext.Session.SetString("UserLogin", user.Login);
        HttpContext.Session.SetString("UserFIO", user.FIO);
        HttpContext.Session.SetString("UserType", user.Type);

        return RedirectToAction("Index", "Requests");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("UserLogin") != null)
            return RedirectToAction("Index", "Requests");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string fio, string phone, string login, string password, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Все поля обязательны для заполнения.";
            return View();
        }

        if (password != confirmPassword)
        {
            ViewBag.Error = "Пароли не совпадают.";
            return View();
        }

        if (await _db.Users.AnyAsync(u => u.Login == login))
        {
            ViewBag.Error = "Пользователь с таким логином уже существует.";
            return View();
        }

        var user = new User
        {
            FIO = fio,
            Phone = phone,
            Login = login,
            Password = password,
            Type = "Заказчик"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        HttpContext.Session.SetInt32("UserID", user.UserID);
        HttpContext.Session.SetString("UserLogin", user.Login);
        HttpContext.Session.SetString("UserFIO", user.FIO);
        HttpContext.Session.SetString("UserType", user.Type);

        return RedirectToAction("Index", "Requests");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

