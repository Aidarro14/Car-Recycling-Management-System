using System.Diagnostics;
using CarRecyclingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Data; // Добавьте это, если ваш DbContext находится здесь
using Microsoft.EntityFrameworkCore; // Добавьте это для ToListAsync() или ToList()

namespace CarRecyclingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // Добавляем приватное поле для DbContext

        // Модифицируем конструктор для внедрения зависимостей DbContext
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context; // Инициализируем DbContext
        }

        public IActionResult Index()
        {
            var reviews = new List<ReviewModel>
            {
                new ReviewModel { Name = "Анна С.", Rating = 5, Date = DateTime.Now.AddDays(-10), Text = "Процесс утилизации был невероятно простым и быстрым! Очень довольна сервисом." },
                new ReviewModel { Name = "Иван П.", Rating = 4, Date = DateTime.Now.AddMonths(-1), Text = "Удобный сервис. Все шаги понятны. Спасибо за компенсацию!" },
                new ReviewModel { Name = "Ольга К.", Rating = 5, Date = DateTime.Now.AddDays(-5), Text = "Заявка была одобрена быстро, пункт приема легко найти. Рекомендую!" }
            };
            ViewBag.Reviews = reviews; // Передаем отзывы в представление через ViewBag

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Points() // Изменяем на асинхронный метод
        {
            // Получаем список всех пунктов утилизации из базы данных
            // Убедитесь, что у вас есть DbSet<RecyclingPoint> RecyclingPoints в вашем AppDbContext
            var recyclingPoints = await _context.RecyclingPoints.ToListAsync(); // Асинхронное получение данных

            // Передаем список пунктов утилизации в представление
            return View(recyclingPoints);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult MyApplications()
        {
            return View();
        }
    }
}