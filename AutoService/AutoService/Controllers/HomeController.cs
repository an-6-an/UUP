using Microsoft.AspNetCore.Mvc;

namespace AutoService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Если пользователь авторизован, отправляем на заявки
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Requests");
            }

            // Иначе показываем приветственную страницу
            return View();
        }
    }
}