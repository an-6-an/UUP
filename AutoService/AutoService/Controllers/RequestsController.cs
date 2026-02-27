using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoService.Models.ViewModels;

namespace AutoService.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(ApplicationDbContext context, ILogger<RequestsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarType)
                .Include(r => r.Client)
                .Include(r => r.Master)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return View(requests);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarType)
                .Include(r => r.Client)
                .Include(r => r.Master)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Master)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clients = new SelectList(
                await _context.Users.Where(u => u.Type == "Заказчик").ToListAsync(),
                "Id", "FIO");

            ViewBag.Cars = new SelectList(
                await _context.Cars.Include(c => c.CarType).ToListAsync(),
                "Id", "CarModel");

            ViewBag.Mechanics = new SelectList(
                await _context.Users.Where(u => u.Type == "Автомеханик").ToListAsync(),
                "Id", "FIO");

            return View();
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,CarId,ProblemDescription,RequestStatus,CompletionDate,RepairParts,MasterId,ClientId")] Request request)
        {
            // Устанавливаем дату создания
            request.CreatedAt = DateTime.Now;

            // Валидация
            if (request.MasterId == 0) request.MasterId = null;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(request);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Заявка успешно создана!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при создании заявки");
                    ModelState.AddModelError("", "Произошла ошибка при сохранении. Попробуйте еще раз.");
                }
            }

            // Если дошли до сюда - что-то пошло не так, перезагружаем списки
            ViewBag.Clients = new SelectList(
                await _context.Users.Where(u => u.Type == "Заказчик").ToListAsync(),
                "Id", "FIO", request.ClientId);

            ViewBag.Cars = new SelectList(
                await _context.Cars.Include(c => c.CarType).ToListAsync(),
                "Id", "CarModel", request.CarId);

            ViewBag.Mechanics = new SelectList(
                await _context.Users.Where(u => u.Type == "Автомеханик").ToListAsync(),
                "Id", "FIO", request.MasterId);

            return View(request);
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            ViewBag.Clients = new SelectList(
                await _context.Users.Where(u => u.Type == "Заказчик").ToListAsync(),
                "Id", "FIO", request.ClientId);

            ViewBag.Cars = new SelectList(
                await _context.Cars.Include(c => c.CarType).ToListAsync(),
                "Id", "CarModel", request.CarId);

            ViewBag.Mechanics = new SelectList(
                await _context.Users.Where(u => u.Type == "Автомеханик").ToListAsync(),
                "Id", "FIO", request.MasterId);

            ViewBag.Statuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Новая заявка", Text = "Новая заявка" },
                new SelectListItem { Value = "В процессе ремонта", Text = "В процессе ремонта" },
                new SelectListItem { Value = "Готова к выдаче", Text = "Готова к выдаче" },
                new SelectListItem { Value = "Отменена", Text = "Отменена" }
            };

            return View(request);
        }

        // POST: Requests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,CarId,ProblemDescription,RequestStatus,CompletionDate,RepairParts,MasterId,ClientId,CreatedAt")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            // Устанавливаем дату обновления
            request.UpdatedAt = DateTime.Now;

            if (request.MasterId == 0) request.MasterId = null;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Заявка успешно обновлена!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RequestExists(request.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при обновлении заявки");
                    ModelState.AddModelError("", "Произошла ошибка при сохранении. Попробуйте еще раз.");
                }
            }

            // Перезагружаем списки
            ViewBag.Clients = new SelectList(
                await _context.Users.Where(u => u.Type == "Заказчик").ToListAsync(),
                "Id", "FIO", request.ClientId);

            ViewBag.Cars = new SelectList(
                await _context.Cars.Include(c => c.CarType).ToListAsync(),
                "Id", "CarModel", request.CarId);

            ViewBag.Mechanics = new SelectList(
                await _context.Users.Where(u => u.Type == "Автомеханик").ToListAsync(),
                "Id", "FIO", request.MasterId);

            ViewBag.Statuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Новая заявка", Text = "Новая заявка" },
                new SelectListItem { Value = "В процессе ремонта", Text = "В процессе ремонта" },
                new SelectListItem { Value = "Готова к выдаче", Text = "Готова к выдаче" },
                new SelectListItem { Value = "Отменена", Text = "Отменена" }
            };

            return View(request);
        }

        // POST: Requests/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int requestId, string message, int masterId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Комментарий не может быть пустым";
                return RedirectToAction(nameof(Details), new { id = requestId });
            }

            var comment = new Comment
            {
                Message = message,
                MasterId = masterId,
                RequestId = requestId,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Комментарий добавлен";
            return RedirectToAction(nameof(Details), new { id = requestId });
        }

        // GET: Requests/Statistics
        public async Task<IActionResult> Statistics()
        {
            var requests = await _context.Requests
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarType)
                .ToListAsync();

            var stats = new StatisticsViewModel
            {
                TotalRequests = requests.Count,
                NewRequests = requests.Count(r => r.RequestStatus == "Новая заявка"),
                InProgressRequests = requests.Count(r => r.RequestStatus == "В процессе ремонта"),
                CompletedRequests = requests.Count(r => r.RequestStatus == "Готова к выдаче"),
                CancelledRequests = requests.Count(r => r.RequestStatus == "Отменена"),

                // Статистика по типам неисправностей (по описанию проблемы)
                ProblemsByType = requests
                    .GroupBy(r => r.Car?.CarType?.Name ?? "Не указан")
                    .Select(g => new CarTypeStatistic { CarType = g.Key, Count = g.Count() })
                    .ToList(),

                // Среднее время выполнения (для завершенных)
                AverageCompletionDays = requests
                    .Where(r => r.CompletionDate.HasValue && r.StartDate != default)
                    .Select(r => (r.CompletionDate.Value - r.StartDate).Days)
                    .DefaultIfEmpty(0)
                    .Average()
            };

            return View(stats);
        }

        private async Task<bool> RequestExists(int id)
        {
            return await _context.Requests.AnyAsync(r => r.Id == id);
        }
    }
}