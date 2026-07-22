using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CarRecyclingWeb.Models
{
    public class RecyclingPoint
    {
        [Key]
        public int PointId { get; set; }

        [Required(ErrorMessage = "Название пункта обязательно.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Адрес пункта обязателен.")]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        
        public string? MapUrl { get; set; } 

        [StringLength(200)]
        public string? OpeningHours { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; } 
       
        public ICollection<Request> Requests { get; set; }
    }
}