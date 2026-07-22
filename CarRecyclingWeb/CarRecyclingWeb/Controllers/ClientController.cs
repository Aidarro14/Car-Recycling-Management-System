using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Models;
using CarRecyclingWeb.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BCrypt.Net;

namespace CarRecyclingWeb.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterClientModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Clients.AnyAsync(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Пользователь с таким Email уже зарегистрирован.");
                    return View(model);
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var client = new Client
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    PhoneNumber = model.PhoneNumber
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
                    new Claim(ClaimTypes.Name, client.Email),
                    new Claim("FullName", $"{client.FirstName} {client.LastName}"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                TempData["SuccessMessage"] = "Регистрация прошла успешно! Добро пожаловать!";
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ClientLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == model.Email);

                if (client != null && BCrypt.Net.BCrypt.Verify(model.Password, client.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
                        new Claim(ClaimTypes.Name, client.Email),
                        new Claim("FullName", $"{client.FirstName} {client.LastName}")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                    await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                    TempData["SuccessMessage"] = "Вы успешно вошли!";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Неверный Email или пароль.");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null)
            {
                TempData["ErrorMessage"] = "Для доступа к профилю необходимо войти в систему.";
                return RedirectToAction("Login", "Client");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Неверный формат ID пользователя.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            var client = await _context.Clients
                                       .Include(c => c.Cars)
                                       .Include(c => c.Requests)
                                       .FirstOrDefaultAsync(c => c.ClientId == userId);

            if (client == null)
            {
                TempData["ErrorMessage"] = "Информация о вашем профиле не найдена.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            return View(client);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Ошибка идентификации пользователя.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            var client = await _context.Clients.FindAsync(userId);
            if (client == null)
            {
                TempData["ErrorMessage"] = "Профиль не найден.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            var model = new EditClientProfileViewModel
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditClientProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userIdString == null || !int.TryParse(userIdString, out int userId))
                {
                    TempData["ErrorMessage"] = "Ошибка идентификации пользователя.";
                    await HttpContext.SignOutAsync("MyCookieAuth");
                    return RedirectToAction("Login", "Client");
                }

                var client = await _context.Clients.FindAsync(userId);
                if (client == null)
                {
                    TempData["ErrorMessage"] = "Профиль не найден.";
                    await HttpContext.SignOutAsync("MyCookieAuth");
                    return RedirectToAction("Login", "Client");
                }

                if (client.Email != model.Email && await _context.Clients.AnyAsync(c => c.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Этот Email уже используется другим пользователем.");
                    return View(model);
                }

                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.Email = model.Email;
                client.PhoneNumber = model.PhoneNumber;

                _context.Clients.Update(client);
                await _context.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
                    new Claim(ClaimTypes.Name, client.Email),
                    new Claim("FullName", $"{client.FirstName} {client.LastName}"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                TempData["SuccessMessage"] = "Данные профиля успешно обновлены!";
                return RedirectToAction("Profile");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (userIdString == null || !int.TryParse(userIdString, out int userId))
                {
                    TempData["ErrorMessage"] = "Ошибка идентификации пользователя.";
                    await HttpContext.SignOutAsync("MyCookieAuth");
                    return RedirectToAction("Login", "Client");
                }

                var client = await _context.Clients.FindAsync(userId);
                if (client == null)
                {
                    TempData["ErrorMessage"] = "Профиль не найден.";
                    await HttpContext.SignOutAsync("MyCookieAuth");
                    return RedirectToAction("Login", "Client");
                }

                if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, client.PasswordHash))
                {
                    ModelState.AddModelError("OldPassword", "Неверный текущий пароль.");
                    return View(model);
                }

                client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                _context.Clients.Update(client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Пароль успешно изменен!";
                return RedirectToAction("Profile");
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyCars()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Ошибка идентификации пользователя.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            var cars = await _context.Cars
                                     .Where(c => c.ClientId == userId)
                                     .ToListAsync();

            return View(cars);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null || !int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Ошибка идентификации пользователя.";
                await HttpContext.SignOutAsync("MyCookieAuth");
                return RedirectToAction("Login", "Client");
            }

            var requests = await _context.Requests
                                         .Include(r => r.RecyclingPoint)
                                         .Include(r => r.Car)
                                         .Where(r => r.ClientId == userId)
                                         .ToListAsync();

            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return await LogoutPost();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            TempData["SuccessMessage"] = "Вы успешно вышли из аккаунта.";
            return RedirectToAction("Login", "Client");
        }
    }
}