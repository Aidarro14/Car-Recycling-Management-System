using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Models;
using CarRecyclingWeb.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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

        private async Task LoadCommonViewModelData(CreateRequestViewModel viewModel)
        {
            viewModel.RecyclingPoints = await _context.RecyclingPoints
                                                      .Select(rp => new SelectListItem
                                                      {
                                                          Value = rp.PointId.ToString(),
                                                          Text = $"{rp.Name} - {rp.Address}"
                                                      })
                                                      .ToListAsync();

            viewModel.VehicleTypes = Enum.GetValues(typeof(VehicleType))
                                         .Cast<VehicleType>()
                                         .Select(v => new SelectListItem
                                         {
                                             Value = v.ToString(),
                                             Text = v.ToString()
                                         })
                                         .ToList();
        }

        private decimal CalculateRecyclingCost(decimal weightKg, string vehicleType)
        {
            decimal baseRate = 2.5m;
            decimal typeMultiplier = 1.0m;

            switch (vehicleType.ToLower())
            {
                case "грузовой":
                    typeMultiplier = 1.5m;
                    break;
                case "специальный":
                    typeMultiplier = 2.0m;
                    break;
                default:
                    typeMultiplier = 1.0m;
                    break;
            }

            return weightKg * baseRate * typeMultiplier;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CalculateCostApi(decimal weight, string vehicleType)
        {
            if (weight <= 0 || string.IsNullOrEmpty(vehicleType))
            {
                return BadRequest(new { error = "Некорректные входные данные для расчета стоимости." });
            }

            try
            {
                decimal cost = CalculateRecyclingCost(weight, vehicleType);
                return Ok(new { cost = cost.ToString("F2") });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ошибка при расчете стоимости: " + ex.Message });
            }
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateRequestViewModel();
            await LoadCommonViewModelData(viewModel);
            return View(viewModel);
        }

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

            if (!ModelState.IsValid)
            {
                await LoadCommonViewModelData(model);
                return View(model);
            }

            var existingCarByVin = await _context.Cars
                                                 .FirstOrDefaultAsync(c => c.VIN == model.CarVIN);
            if (existingCarByVin != null)
            {
                ModelState.AddModelError("CarVIN", "Автомобиль с таким VIN-номером уже зарегистрирован. Пожалуйста, проверьте VIN.");
                await LoadCommonViewModelData(model);
                return View(model);
            }

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

            decimal? calculatedCost = null;
            if (model.CarWeightKg.HasValue)
            {
                calculatedCost = CalculateRecyclingCost(model.CarWeightKg.Value, model.CarVehicleType.ToString());
            }

            var request = new Request
            {
                ClientId = int.Parse(clientId),
                CarId = car.CarId,
                Condition = model.Condition,
                Description = model.Description,
                PreferredDisposalDate = model.PreferredDisposalDate,
                RecyclingPointId = model.RecyclingPointId,
                Status = RequestStatus.Accepted,
                Cost = calculatedCost
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ваша заявка успешно подана!";
            return RedirectToAction("MyRequests");
        }

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
                .Include(r => r.Feedbacks)
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
                                        .Include(r => r.Client)
                                        .FirstOrDefaultAsync(m => m.RequestId == requestId);

            if (request == null)
            {
                return NotFound();
            }

            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(currentUserIdString, out int currentClientId))
            {
                return Unauthorized();
            }

            if (request.ClientId != currentClientId)
            {
                return Unauthorized();
            }

            if (request.Status != RequestStatus.Completed)
            {
                TempData["ErrorMessage"] = "Отзыв можно оставить только для завершенных заявок.";
                return RedirectToAction(nameof(MyRequests));
            }

            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.RequestId == requestId);
            if (existingFeedback != null)
            {
                TempData["InfoMessage"] = "Вы уже оставили отзыв для этой заявки.";
                return RedirectToAction(nameof(MyRequests));
            }

            var feedbackViewModel = new Feedback
            {
                RequestId = request.RequestId,
                ClientId = request.ClientId
            };

            ViewBag.RequestDetails = $"Заявка №{request.RequestId} ({request.Car?.Brand} {request.Car?.Model})";
            return View(feedbackViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveFeedback(Feedback feedback)
        {
            ModelState.Remove("Client");
            ModelState.Remove("Request");

            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdString))
            {
                return Unauthorized();
            }

            if (!int.TryParse(currentUserIdString, out int currentClientId))
            {
                return Unauthorized();
            }

            var request = await _context.Requests
                                        .FirstOrDefaultAsync(r => r.RequestId == feedback.RequestId &&
                                                                  r.ClientId == currentClientId &&
                                                                  r.Status == RequestStatus.Completed);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Неверная заявка или статус не позволяет оставить отзыв.";
                return RedirectToAction(nameof(MyRequests));
            }

            var existingFeedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.RequestId == feedback.RequestId);
            if (existingFeedback != null)
            {
                TempData["InfoMessage"] = "Отзыв для этой заявки уже существует.";
                return RedirectToAction(nameof(MyRequests));
            }

            if (ModelState.IsValid)
            {
                feedback.SubmissionDate = DateTime.Now;
                _context.Add(feedback);
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
                .Include(r => r.Car)
                .Include(r => r.Client)
                .Include(r => r.RecyclingPoint)
                .Include(r => r.Feedbacks)
                .FirstOrDefaultAsync(m => m.RequestId == id);

            if (request == null)
            {
                return NotFound();
            }

            var currentUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(currentUserIdString) && int.TryParse(currentUserIdString, out int currentClientId))
            {
                if (request.ClientId != currentClientId)
                {
                    return Forbid();
                }
            }
            else
            {
                return Unauthorized();
            }

            return View(request);
        }
    }
}