using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Models;
using CarRecyclingWeb.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq; // Для метода .Select

namespace CarRecyclingWeb.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // Вспомогательный метод для загрузки общих данных в ViewModel
        private async Task LoadCommonViewModelData(CreateRequestViewModel viewModel)
        {
            viewModel.RecyclingPoints = await _context.RecyclingPoints
                                                      .Select(rp => new SelectListItem
                                                      {
                                                          Value = rp.PointId.ToString(),
                                                          Text = $"{rp.Name} - {rp.Address}"
                                                      })
                                                      .ToListAsync();

            // Загружаем типы автомобилей из Enum
            viewModel.VehicleTypes = Enum.GetValues(typeof(VehicleType))
                                         .Cast<VehicleType>()
                                         .Select(v => new SelectListItem
                                         {
                                             Value = v.ToString(),
                                             Text = v.ToString() // Или используйте атрибут [Display] для локализации
                                         })
                                         .ToList();
        }
        private decimal CalculateRecyclingCost(decimal weightKg, string vehicleType)
        {
            decimal baseRate = 2.5m; // руб/кг
            decimal typeMultiplier = 1.0m;

            switch (vehicleType.ToLower())
            {
                case "грузовой":
                    typeMultiplier = 1.5m;
                    break;
                case "специальный": // Исправлено на "специальный", чтобы соответствовало Enum
                    typeMultiplier = 2.0m;
                    break;
                default: // легковой
                    typeMultiplier = 1.0m;
                    break;
            }

            return weightKg * baseRate * typeMultiplier;
        }
        [HttpGet] // Используем HttpGet, так как это получение данных
        [AllowAnonymous] // Разрешить неавторизованным пользователям доступ к этому методу, если нужно,
                         // иначе [Authorize]
        public IActionResult CalculateCostApi(decimal weight, string vehicleType)
        {
            if (weight <= 0 || string.IsNullOrEmpty(vehicleType))
            {
                // Возвращаем ошибку, если входные данные некорректны
                return BadRequest(new { error = "Некорректные входные данные для расчета стоимости." });
            }

            try
            {
                // Используем уже существующий приватный метод для расчета
                decimal cost = CalculateRecyclingCost(weight, vehicleType);
                return Ok(new { cost = cost.ToString("F2") }); // Возвращаем стоимость в формате JSON, F2 для 2 знаков после запятой
            }
            catch (Exception ex)
            {
                // Логирование ошибки, если что-то пошло не так
                return StatusCode(500, new { error = "Ошибка при расчете стоимости: " + ex.Message });
            }
        }

        // GET: Requests/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateRequestViewModel();
            await LoadCommonViewModelData(viewModel); // Загружаем данные для выпадающих списков
            return View(viewModel);
        }

        // POST: Requests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestViewModel model)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(clientId))
            {
                TempData["ErrorMessage"] = "Вы не авторизованы. Пожалуйста, войдите в систему.";
                return RedirectToAction("Login", "Client");
            }

            // ***** Валидация модели и проверка на дубликаты VIN и LicensePlate *****

            if (!ModelState.IsValid)
            {
                await LoadCommonViewModelData(model); // Перезагружаем данные для выпадающих списков
                return View(model);
            }

            // Проверяем, существует ли автомобиль с таким VIN-номером
            var existingCarByVin = await _context.Cars
                                                 .FirstOrDefaultAsync(c => c.VIN == model.CarVIN);
            if (existingCarByVin != null)
            {
                ModelState.AddModelError("CarVIN", "Автомобиль с таким VIN-номером уже зарегистрирован. Пожалуйста, проверьте VIN.");
                await LoadCommonViewModelData(model);
                return View(model);
            }

            // Проверяем, существует ли автомобиль с таким государственным номером
            var existingCarByLicensePlate = await _context.Cars
                                                          .FirstOrDefaultAsync(c => c.LicensePlate == model.CarLicensePlate);
            if (existingCarByLicensePlate != null)
            {
                ModelState.AddModelError("CarLicensePlate", "Автомобиль с таким государственным номером уже зарегистрирован. Пожалуйста, проверьте госномер.");
                await LoadCommonViewModelData(model);
                return View(model);
            }

            var car = new Car
            {
                ClientId = int.Parse(clientId),
                Brand = model.CarBrand,
                Model = model.CarModel,
                Year = model.CarYear,
                VIN = model.CarVIN,
                LicensePlate = model.CarLicensePlate,
                WeightKg = model.CarWeightKg,
                VehicleType = model.CarVehicleType
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            // *** Расчет стоимости перед созданием заявки ***
            decimal? calculatedCost = null;
            if (model.CarWeightKg.HasValue) // Проверяем, что вес указан
            {
                calculatedCost = CalculateRecyclingCost(model.CarWeightKg.Value, model.CarVehicleType.ToString());
            }

            // Создаем новую заявку
            var request = new Request
            {
                ClientId = int.Parse(clientId),
                CarId = car.CarId,
                Condition = model.Condition,
                Description = model.Description,
                PreferredDisposalDate = model.PreferredDisposalDate,
                RecyclingPointId = model.RecyclingPointId,
                Status = RequestStatus.Accepted,
                Cost = calculatedCost // <-- Присваиваем рассчитанную стоимость
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ваша заявка успешно подана!";
            return RedirectToAction("MyRequests");
        }

        // GET: Requests/MyRequests
        // В RequestsController.cs

        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString) || !int.TryParse(currentUserIdString, out int currentClientId))
            {
                return Unauthorized();
            }

            var requests = await _context.Requests
                .Where(r => r.ClientId == currentClientId)
                .Include(r => r.Car)
                .Include(r => r.RecyclingPoint)
                // Загружаем связанные отзывы, чтобы проверить их наличие
                .Include(r => r.Feedbacks) // <-- ДОБАВЬТЕ ЭТУ СТРОКУ
                .OrderByDescending(r => r.SubmissionDate)
                .ToListAsync();

            return View(requests);
        }
        public async Task<IActionResult> LeaveFeedback(int? requestId)
        {
            if (requestId == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                                        .Include(r => r.Client) // Убедитесь, что загружаете клиента
                                        .FirstOrDefaultAsync(m => m.RequestId == requestId);

            if (request == null)
            {
                return NotFound();
            }

            // Получаем ID текущего клиента из Claims
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString))
            {
                // Пользователь не авторизован
                return Unauthorized();
            }

            // Преобразуем string ID пользователя в int
            if (!int.TryParse(currentUserIdString, out int currentClientId))
            {
                // Если ID пользователя не является числом (что маловероятно для числовых ID),
                // или если ID не может быть преобразован
                return Unauthorized(); // Или другой обработчик ошибки
            }

            // Проверяем, что заявка принадлежит текущему авторизованному клиенту
            if (request.ClientId != currentClientId) // Здесь оба уже int, сравнение корректно
            {
                return Unauthorized();
            }

            // Проверить, что заявка действительно завершена
            if (request.Status != RequestStatus.Completed)
            {
                TempData["ErrorMessage"] = "Отзыв можно оставить только для завершенных заявок.";
                return RedirectToAction(nameof(MyRequests));
            }

            // Проверить, что отзыв ещё не был оставлен для этой заявки
            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.RequestId == requestId);
            if (existingFeedback != null)
            {
                TempData["InfoMessage"] = "Вы уже оставили отзыв для этой заявки.";
                return RedirectToAction(nameof(MyRequests));
            }

            var feedbackViewModel = new Feedback
            {
                RequestId = request.RequestId,
                ClientId = request.ClientId // RequestId и ClientId уже int
            };

            ViewBag.RequestDetails = $"Заявка №{request.RequestId} ({request.Car?.Brand} {request.Car?.Model})";
            return View(feedbackViewModel);
        }

        // POST: Requests/LeaveFeedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveFeedback(Feedback feedback)
        {
            ModelState.Remove("Client");
            ModelState.Remove("Request");
            // Получаем ID текущего клиента для проверки безопасности
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(currentUserIdString, out int currentClientId))
            {
                return Unauthorized();
            }

            // Дополнительная проверка, чтобы убедиться, что RequestId принадлежит текущему клиенту и завершен
            var request = await _context.Requests
                                        .FirstOrDefaultAsync(r => r.RequestId == feedback.RequestId &&
                                                                  r.ClientId == currentClientId && // Здесь ClientId уже int
                                                                  r.Status == RequestStatus.Completed);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Неверная заявка или статус не позволяет оставить отзыв.";
                return RedirectToAction(nameof(MyRequests));
            }

            // Проверить, что отзыв ещё не был оставлен для этой заявки
            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.RequestId == feedback.RequestId);
            if (existingFeedback != null)
            {
                TempData["InfoMessage"] = "Отзыв для этой заявки уже существует.";
                return RedirectToAction(nameof(MyRequests));
            }

            if (ModelState.IsValid)
            {
                feedback.SubmissionDate = DateTime.Now;
                _context.Add(feedback); // сохраняется Comment
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ваш отзыв успешно оставлен!";
                return RedirectToAction(nameof(MyRequests));
            }


            ViewBag.RequestDetails = $"Заявка №{request.RequestId} ({request.Car?.Brand} {request.Car?.Model})";
            return View(feedback);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests
                .Include(r => r.Car) // Включите автомобиль, чтобы показать его детали
                .Include(r => r.Client) // Включите клиента
                .Include(r => r.RecyclingPoint) // Включите пункт утилизации
                .Include(r => r.Feedbacks) // Включите отзывы, если нужно
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            // Проверка, что заявка принадлежит текущему авторизованному клиенту,
            // если это страница деталей для клиента
            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(currentUserIdString) && int.TryParse(currentUserIdString, out int currentClientId))
            {
                if (request.ClientId != currentClientId)
                {
                    // Если пользователь не владелец заявки, и он не админ/работник (если есть роли)
                    // Можно перенаправить на страницу "Мои заявки" или вернуть Unauthorized
                    return Forbid(); // Запрещено
                }
            }
            else
            {
                // Если не авторизован или ID некорректен
                return Unauthorized();
            }

            return View(request);
        }
    }
}