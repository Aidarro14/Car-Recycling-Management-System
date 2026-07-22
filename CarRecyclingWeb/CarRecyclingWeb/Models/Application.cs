using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRecyclingWeb.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пользователь обязателен.")]
        public string ClientId { get; set; }

        [Required(ErrorMessage = "Марка автомобиля обязательна.")]
        [StringLength(100, ErrorMessage = "Марка не может превышать 100 символов.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Модель автомобиля обязательна.")]
        [StringLength(100, ErrorMessage = "Модель не может превышать 100 символов.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Год выпуска обязателен.")]
        [Range(1900, 2025, ErrorMessage = "Год выпуска должен быть в диапазоне от 1900 до текущего года.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "VIN-номер обязателен.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN-номер должен содержать 17 символов.")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "Госномер обязателен.")]
        [StringLength(10, ErrorMessage = "Госномер не может превышать 10 символов.")]
        public string LicensePlate { get; set; }

        [Required(ErrorMessage = "Пункт утилизации обязателен.")]
        public int RecyclingPointId { get; set; }

        public string Status { get; set; } = "Новая";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public string RecyclingPointName { get; set; }

        [NotMapped]
        public string ClientName { get; set; }
    }
}