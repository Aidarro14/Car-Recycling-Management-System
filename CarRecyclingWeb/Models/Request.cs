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

        [EnumMember(Value = "Ожидает подтверждения")] // Это, скорее всего, для worker'а
        [Display(Name = "Ожидает подтверждения")]
        AwaitingConfirmation,

        [EnumMember(Value = "Требует доработки")]
        [Display(Name = "Требует доработки")]
        NeedsRevision,

        // Этот статус, как я понял, может быть эквивалентен "Ожидает подтверждения", но для админа.
        // Если "Awaiting Admin Approval" — это точное строковое значение в БД, то оставляем так.
        // Иначе, если он совпадает с "Ожидает подтверждения", этот элемент может быть не нужен или должен иметь такое же EnumMember Value.
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

        // --- Убедитесь, что эти три поля присутствуют в вашем Request.cs ---
        [Display(Name = "Предпочитаемая дата утилизации")] // Добавьте этот атрибут, он полезен для UI
        public DateTime PreferredDisposalDate { get; set; } // Это то, что вы добавляете из ViewModel

        [Display(Name = "Состояние автомобиля")] // Добавьте этот атрибут
        public string Condition { get; set; } // Это то, что вы добавляете из ViewModel

        [Display(Name = "Дополнительное описание")] // Добавьте этот атрибут
        [StringLength(500)] // Ограничение длины строки
        public string? Description { get; set; } // Nullable, если необязательно

        public DateTime? CompletionDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Cost { get; set; }

        public bool AdminConfirmed { get; set; } = false;

        public string? WorkerComment { get; set; }

        public string? AdminComment { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }

    }
}