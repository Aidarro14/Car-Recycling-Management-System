using System.ComponentModel.DataAnnotations;

namespace CarRecyclingWeb.Models 
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Текущий пароль обязателен.")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")] 
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль обязателен.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Новый пароль должен быть не менее {2} и не более {1} символов.")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")] 
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и подтверждение не совпадают.")]
        [Display(Name = "Подтверждение нового пароля")]
        public string ConfirmPassword { get; set; }
    }
}