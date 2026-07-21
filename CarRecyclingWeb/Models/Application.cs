using System;
using System.ComponentModel.DataAnnotations; // Для валидации
using System.ComponentModel.DataAnnotations.Schema; // Для атрибута NotMapped

namespace CarRecyclingWeb.Models
{
    public class Application
    {
        public int Id { get; set; } // Уникальный идентификатор заявки

        // Внешний ключ для связи с пользователем
        [Required(ErrorMessage = "Пользователь обязателен.")]
        public string ClientId { get; set; } // ID пользователя (например, из IdentityUser)

        // Информация об автомобиле
        [Required(ErrorMessage = "Марка автомобиля обязательна.")]
        [StringLength(100, ErrorMessage = "Марка не может превышать 100 символов.")]
        public string Brand { get; set; } // Марка автомобиля

        [Required(ErrorMessage = "Модель автомобиля обязательна.")]
        [StringLength(100, ErrorMessage = "Модель не может превышать 100 символов.")]
        public string Model { get; set; } // Модель автомобиля

        [Required(ErrorMessage = "Год выпуска обязателен.")]
        [Range(1900, 2025, ErrorMessage = "Год выпуска должен быть в диапазоне от 1900 до текущего года.")]
        public int Year { get; set; } // Год выпуска

        [Required(ErrorMessage = "VIN-номер обязателен.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN-номер должен содержать 17 символов.")]
        public string VIN { get; set; } // VIN-номер

        [Required(ErrorMessage = "Госномер обязателен.")]
        [StringLength(10, ErrorMessage = "Госномер не может превышать 10 символов.")]
        public string LicensePlate { get; set; } // Госномер

        // Информация о пункте утилизации
        [Required(ErrorMessage = "Пункт утилизации обязателен.")]
        public int RecyclingPointId { get; set; } // ID выбранного пункта утилизации

        // Статус заявки
        public string Status { get; set; } = "Новая"; // Статус заявки (Новая, В обработке, Завершена, Отклонена)

        // Дата создания заявки
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Навигационные свойства (если у вас есть соответствующие модели)
        // [ForeignKey("ClientId")]
        // public virtual ApplicationUser Client { get; set; } // Если у вас есть кастомная модель ApplicationUser
        // [ForeignKey("RecyclingPointId")]
        // public virtual RecyclingPoint RecyclingPoint { get; set; } // Если у вас есть модель RecyclingPoint

        // Добавим свойство для вывода имени пункта утилизации, если нет навигационного свойства
        [NotMapped] // Это свойство не будет маппиться в базу данных
        public string RecyclingPointName { get; set; }

        // Добавим свойство для имени клиента, если нет навигационного свойства
        [NotMapped]
        public string ClientName { get; set; }
    }

   
}