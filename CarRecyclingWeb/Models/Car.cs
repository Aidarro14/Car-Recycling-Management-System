using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CarRecyclingWeb.Models
{
    public enum VehicleType // Добавляем перечисление для типа автомобиля
    {
        Легковой,
        Грузовой,
        Специальный
    }

    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Марка автомобиля обязательна.")]
        [StringLength(100)]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Модель автомобиля обязательна.")]
        [StringLength(100)]
        public string Model { get; set; }

        [Required(ErrorMessage = "Год выпуска обязателен.")]
        [Range(1900, 2025, ErrorMessage = "Год выпуска должен быть в диапазоне от 1900 до текущего года.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "VIN обязателен.")]
        [StringLength(50)]
        public string VIN { get; set; }

        [Required(ErrorMessage = "Госномер обязателен.")]
        [StringLength(10)]
        public string LicensePlate { get; set; }

        // --- НОВЫЕ ПОЛЯ ---
        [Display(Name = "Вес (кг)")]
        [Column(TypeName = "DECIMAL(10,2)")] // Указываем тип колонки в БД
        [Range(0.01, 100000.00, ErrorMessage = "Вес должен быть положительным числом.")]
        public decimal? WeightKg { get; set; } // Делаем nullable, так как в БД может быть NULL

        [Display(Name = "Тип автомобиля")]
        [EnumDataType(typeof(VehicleType), ErrorMessage = "Некорректный тип автомобиля.")]
        public VehicleType VehicleType { get; set; } = VehicleType.Легковой; // Значение по умолчанию

        [Required(ErrorMessage = "Клиент обязателен.")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public ICollection<Request> Requests { get; set; }
    }
}