using System.ComponentModel.DataAnnotations;

namespace CarRecyclingWeb.Models
{
    public class RegisterClientModel
    {
        [Required(ErrorMessage = "Имя обязательно.")]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Неверный формат Email.")]
        [StringLength(255)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        [StringLength(20)] // <--- ДОБАВЬТЕ ЭТО
        [Display(Name = "Номер телефона (необязательно)")] // <--- ДОБАВЬТЕ ЭТО
        public string? PhoneNumber { get; set; } // <--- ДОБАВЬТЕ ЭТО
    }
}