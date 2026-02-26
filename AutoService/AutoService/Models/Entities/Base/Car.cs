using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("Cars")]
    public class Car : BaseEntity
    {
        [Required]
        [Display(Name = "Тип автомобиля")]
        public int CarTypeId { get; set; }

        [Required]
        [Display(Name = "Модель автомобиля")]
        [StringLength(100)]
        public string CarModel { get; set; } = string.Empty; // Вместо Brand + Model

        [ForeignKey("CarTypeId")]
        public virtual CarType CarType { get; set; } = null!;

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}