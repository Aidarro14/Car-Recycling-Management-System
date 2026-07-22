using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CarRecyclingWeb.Models
{
    public enum RequestStatus
    {
        [EnumMember(Value = "Принята")]
        [Display(Name = "Принята")]
        Accepted,

        [EnumMember(Value = "В обработке")]
        [Display(Name = "В обработке")]
        InProgress,

        [EnumMember(Value = "На утилизации")]
        [Display(Name = "На утилизации")]
        OnRecycling,

        [EnumMember(Value = "Завершена")]
        [Display(Name = "Завершена")]
        Completed,

        [EnumMember(Value = "Ожидает подтверждения")]
        [Display(Name = "Ожидает подтверждения")]
        AwaitingConfirmation,

        [EnumMember(Value = "Требует доработки")]
        [Display(Name = "Требует доработки")]
        NeedsRevision,

        [EnumMember(Value = "Awaiting Admin Approval")]
        [Display(Name = "Ожидает одобрения администратора")]
        AwaitingAdminApproval
    }

    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Автомобиль обязателен.")]
        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public virtual Car Car { get; set; }

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [Required(ErrorMessage = "Пункт утилизации обязателен.")]
        public int RecyclingPointId { get; set; }
        [ForeignKey("RecyclingPointId")]
        public virtual RecyclingPoint RecyclingPoint { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [Required(ErrorMessage = "Статус заявки обязателен.")]
        public RequestStatus Status { get; set; } = RequestStatus.Accepted;

        public DateTime SubmissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Предпочитаемая дата утилизации")]
        public DateTime PreferredDisposalDate { get; set; }

        [Display(Name = "Состояние автомобиля")]
        public string Condition { get; set; }

        [Display(Name = "Дополнительное описание")]
        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime? CompletionDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Cost { get; set; }

        public bool AdminConfirmed { get; set; } = false;

        public string? WorkerComment { get; set; }

        public string? AdminComment { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}