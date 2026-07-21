using System.ComponentModel.DataAnnotations; // Для [Required] и [EmailAddress]

namespace CarRecyclingWeb.Controllers
{
    public class ClientLoginViewModel
    {
        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Неверный формат Email.")]
        [Display(Name = "Email")] // Для лучшего отображения в UI
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; } // Если вы хотите добавить эту функцию

        public string? ReturnUrl { get; set; } // <--- Важно для перенаправления после входа
    }
}