using System.ComponentModel.DataAnnotations;

namespace CarRecyclingWeb.Controllers
{
    public class ClientLoginViewModel
    {
        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Неверный формат Email.")]
        [Display(Name = "Email")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; } 

        public string? ReturnUrl { get; set; } 
    }
}