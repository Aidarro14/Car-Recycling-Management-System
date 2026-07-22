using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CarRecyclingWeb.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

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

        [Required(ErrorMessage = "Пароль обязателен.")]
        [StringLength(255)] 
        public string PasswordHash { get; set; }

        
        [StringLength(20)]
        public string? PhoneNumber { get; set; } 

        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<Request> Requests { get; set; } = new List<Request>();

    }
}