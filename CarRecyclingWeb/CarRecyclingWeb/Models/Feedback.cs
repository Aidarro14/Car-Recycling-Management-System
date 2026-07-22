using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRecyclingWeb.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [Required(ErrorMessage = "Пожалуйста, поставьте оценку.")]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Пожалуйста, оставьте комментарий.")]
        [StringLength(500, ErrorMessage = "Комментарий не должен превышать 500 символов.")]
        public string Comment { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}