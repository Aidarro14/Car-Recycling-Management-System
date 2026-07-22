using System.Diagnostics;
using CarRecyclingWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRecyclingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var reviews = new List<ReviewModel>
            {
                new ReviewModel { Name = "Анна С.", Rating = 5, Date = DateTime.Now.AddDays(-10), Text = "Процесс утилизации был невероятно простым и быстрым! Очень довольна сервисом." },
                new ReviewModel { Name = "Иван П.", Rating = 4, Date = DateTime.Now.AddMonths(-1), Text = "Удобный сервис. Все шаги понятны. Спасибо за компенсацию!" },
                new ReviewModel { Name = "Ольга К.", Rating = 5, Date = DateTime.Now.AddDays(-5), Text = "Заявка была одобрена быстро, пункт приема легко найти. Рекомендую!" }
            };
            ViewBag.Reviews = reviews;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Points()
        {
            var recyclingPoints = await _context.RecyclingPoints.ToListAsync();
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