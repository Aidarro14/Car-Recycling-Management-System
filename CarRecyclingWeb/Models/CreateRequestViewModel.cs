using System; // Для DateTime
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Для List
using Microsoft.AspNetCore.Mvc.Rendering; // Для SelectList

namespace CarRecyclingWeb.Models
{
    public class CreateRequestViewModel
    {
        // --- Поля для создания НОВОГО АВТОМОБИЛЯ ---
        [Required(ErrorMessage = "Марка автомобиля обязательна.")]
        [StringLength(100)]
        [Display(Name = "Марка автомобиля")]
        public string CarBrand { get; set; }

        [Required(ErrorMessage = "Модель автомобиля обязательна.")]
        [StringLength(100)]
        [Display(Name = "Модель автомобиля")]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "Год выпуска обязателен.")]
        [Range(1900, 2025, ErrorMessage = "Год выпуска должен быть в диапазоне от 1900 до текущего года.")]
        [Display(Name = "Год выпуска")]
        public int CarYear { get; set; }

        [Required(ErrorMessage = "VIN-номер обязателен.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN-номер должен содержать 17 символов.")]
        [Display(Name = "VIN-номер")]
        public string CarVIN { get; set; }

        [Required(ErrorMessage = "Госномер обязателен.")]
        [StringLength(10, ErrorMessage = "Госномер не может превышать 10 символов.")]
        [Display(Name = "Госномер")]
        public string CarLicensePlate { get; set; }

        // --- НОВЫЕ ПОЛЯ ДЛЯ АВТОМОБИЛЯ В VIEWMODEL ---
        [Display(Name = "Вес автомобиля (кг)")]
        [Range(0.01, 100000.00, ErrorMessage = "Вес должен быть положительным числом.")]
        public decimal? CarWeightKg { get; set; } // Соответствует WeightKg в Car.cs

        [Display(Name = "Тип автомобиля")]
        [Required(ErrorMessage = "Тип автомобиля обязателен.")]
        public VehicleType CarVehicleType { get; set; } // Соответствует VehicleType в Car.cs

        // Это свойство будет использоваться для заполнения выпадающего списка типов автомобилей
        public List<SelectListItem>? VehicleTypes { get; set; }

        // --- Поля для ЗАЯВКИ (Request) ---
        [Required(ErrorMessage = "Пункт утилизации обязателен.")]
        [Display(Name = "Пункт утилизации")]
        public int RecyclingPointId { get; set; }

        // Это свойство будет использоваться для заполнения выпадающего списка в представлении
        public List<SelectListItem>? RecyclingPoints { get; set; }

        [Display(Name = "Состояние автомобиля")]
        [Required(ErrorMessage = "Пожалуйста, укажите состояние автомобиля.")]
        public string Condition { get; set; } = "Рабочее"; // Значение по умолчанию

        [Display(Name = "Дополнительное описание")]
        [StringLength(500, ErrorMessage = "Описание не может превышать 500 символов.")]
        public string? Description { get; set; } // Nullable, так как может быть не заполнено

        [Display(Name = "Предпочитаемая дата утилизации")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PreferredDisposalDate { get; set; } = DateTime.Today.AddDays(7); // Значение по умолчанию
    }
}