using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CarRecyclingWeb.Models
{
    public enum EmployeeRole
    {
        worker,
        admin
    }

    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Имя обязательно.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Роль обязательна.")]
        public EmployeeRole Role { get; set; } = EmployeeRole.worker;

        public ICollection<Request> Requests { get; set; }
    }
}