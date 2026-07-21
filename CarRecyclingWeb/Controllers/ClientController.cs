using Microsoft.AspNetCore.Mvc;
using CarRecyclingWeb.Models; // Убедитесь, что это присутствует
using CarRecyclingWeb.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; // Для [Authorize]
using Microsoft.EntityFrameworkCore; // <--- ДОБАВЬТЕ ЭТОТ USING для FirstOrDefaultAsync, Any, etc.
using System.Linq; // <--- ДОБАВЬТЕ ЭТОТ USING для Any()

// Убедитесь, что вы установили этот пакет через NuGet: Install-Package BCrypt.Net-Core
// Если вы используете другую библиотеку для хеширования, измените это
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

        // GET: /Client/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Client/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterClientModel model) // Используем RegisterClientModel
        {
            if (ModelState.IsValid)
            {
                // Проверяем, существует ли пользователь с таким email
                if (await _context.Clients.AnyAsync(c => c.Email == model.Email)) // Используем AnyAsync
                {
                    ModelState.AddModelError("Email", "Пользователь с таким Email уже зарегистрирован.");
                    return View(model);
                }

                // Хешируем пароль перед сохранением!
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password); // Вызов правильный

                var client = new Client
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    PhoneNumber = model.PhoneNumber // <--- Добавляем PhoneNumber
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Опционально: сразу же аутентифицировать пользователя после регистрации
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()), // <--- Используем client.ClientId
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

        // GET: /Client/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Client/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ClientLoginViewModel model) // Используем ClientLoginViewModel
        {
            if (ModelState.IsValid)
            {
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == model.Email); // Используем FirstOrDefaultAsync

                if (client != null && BCrypt.Net.BCrypt.Verify(model.Password, client.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()), // <--- Используем client.ClientId
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

        // GET: /Client/Profile
        [Authorize] // Только авторизованные пользователи могут получить доступ к профилю
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Получаем ID текущего авторизованного пользователя из Claims
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userIdString == null)
            {
                TempData["ErrorMessage"] = "Для доступа к профилю необходимо войти в систему.";
                return RedirectToAction("Login", "Client");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Неверный формат ID пользователя.";
                await HttpContext.SignOutAsync("MyCookieAuth"); // Выходим из системы
                return RedirectToAction("Login", "Client");
            }

            // --- ИЗМЕНЕНИЕ ЗДЕСЬ: ДОБАВЛЯЕМ .Include() ---
            var client = await _context.Clients
                                       .Include(c => c.Cars)      // Загружаем связанные автомобили
                                       .Include(c => c.Requests)  // Загружаем связанные заявки
                                       .FirstOrDefaultAsync(c => c.ClientId == userId);

            if (client == null)
            {
                TempData["ErrorMessage"] = "Информация о вашем профиле не найдена.";
                await HttpContext.SignOutAsync("MyCookieAuth"); // Выходим из системы
                return RedirectToAction("Login", "Client");
            }

            // Передаем объект клиента в представление
            return View(client);
        }

        // GET: /Client/EditProfile
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

            // Создаем ViewModel для редактирования (если отличается от Client)
            // или просто передаем модель Client
            var model = new EditClientProfileViewModel // Мы создадим эту ViewModel ниже
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };
            return View(model);
        }

        // POST: /Client/EditProfile
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

                // Проверяем, не занят ли новый email другим пользователем
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

                // Обновляем Claims пользователя, если Email или FullName изменились
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, client.ClientId.ToString()),
                    new Claim(ClaimTypes.Name, client.Email),
                    new Claim("FullName", $"{client.FirstName} {client.LastName}"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));


                TempData["SuccessMessage"] = "Данные профиля успешно обновлены!";
                return RedirectToAction("Profile"); // Перенаправляем обратно на страницу профиля
            }
            return View(model);
        }

        // GET: /Client/ChangePassword
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Client/ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) // Мы создадим эту ViewModel ниже
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

                // Проверяем старый пароль
                if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, client.PasswordHash))
                {
                    ModelState.AddModelError("OldPassword", "Неверный текущий пароль.");
                    return View(model);
                }

                // Хешируем новый пароль
                client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                _context.Clients.Update(client);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Пароль успешно изменен!";
                return RedirectToAction("Profile"); // Перенаправляем обратно на страницу профиля
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

        // GET: /Client/MyRequests
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
                                         .Include(r => r.RecyclingPoint) // Важно для отображения имени пункта
                                         .Include(r => r.Car)            // Важно для отображения данных автомобиля
                                         .Where(r => r.ClientId == userId)
                                         .ToListAsync();

            return View(requests);
        }

        // GET: /Client/Logout
        [HttpGet]
        // Убрали 'async Task<IActionResult>' и 'return View();'
        public async Task<IActionResult> Logout()
        {
            // Сразу вызываем POST-метод для выхода
            // Это автоматически выполнит SignOutAsync и перенаправит на страницу логина
            return await LogoutPost();
        }

        // POST: /Client/Logout - ЭТОТ МЕТОД ОБРАБАТЫВАЕТ ВЫХОД
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize] // Можно оставить [Authorize] здесь, если вы хотите, чтобы только авторизованные могли "выходить"
        public async Task<IActionResult> LogoutPost() // Метод, который фактически выполняет выход
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            TempData["SuccessMessage"] = "Вы успешно вышли из аккаунта.";
            return RedirectToAction("Login", "Client"); // Перенаправляем на страницу логина
        }


        // POST: /Client/Logout - ЭТОТ МЕТОД ДОЛЖЕН ОБРАБАТЫВАТЬ ВЫХОД
       
    }
}