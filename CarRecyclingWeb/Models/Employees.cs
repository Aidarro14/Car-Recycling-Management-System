using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Для навигационных свойств

namespace CarRecyclingWeb.Models
{
    public enum EmployeeRole
    {
        worker, // Соответствует ENUM('worker')
        admin   // Соответствует ENUM('admin')
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
        public EmployeeRole Role { get; set; } = EmployeeRole.worker; // Установка значения по умолчанию

        // Навигационное свойство для связи один-ко-многим с Requests
        public ICollection<Request> Requests { get; set; }
    }
}