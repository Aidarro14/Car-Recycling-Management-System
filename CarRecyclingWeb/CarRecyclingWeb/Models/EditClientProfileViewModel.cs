using System.ComponentModel.DataAnnotations;


namespace CarRecyclingWeb.Models
{
    public class EditClientProfileViewModel
    {
        [Required(ErrorMessage = "Имя обязательно.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}