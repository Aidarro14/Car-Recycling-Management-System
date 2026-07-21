// Models/Feedback.cs
using System.ComponentModel.DataAnnotations; // Добавьте, если нет
using System.ComponentModel.DataAnnotations.Schema; // Добавьте, если нет

namespace CarRecyclingWeb.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        // Это свойство будет внешним ключом, ссылающимся на RequestId в таблице Requests
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        // Если у вас также есть связь с Client в Feedback, убедитесь, что она тоже правильная
        public int ClientId { get; set; } // Это свойство, вероятно, уже есть

        [ForeignKey("ClientId")] // Убедитесь, что это тоже есть
        public virtual Client Client { get; set; } // Это тоже

        // ... остальные свойства отзыва (Rating, Comment, SubmissionDate и т.д.) ...

        [Required(ErrorMessage = "Пожалуйста, поставьте оценку.")]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Пожалуйста, оставьте комментарий.")]
        [StringLength(500, ErrorMessage = "Комментарий не должен превышать 500 символов.")]
        public string Comment { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}