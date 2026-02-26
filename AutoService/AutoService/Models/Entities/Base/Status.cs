using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("Statuses")]
    public class Status : BaseEntity
    {
        [Required]
        [Display(Name = "Статус")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Новая заявка, В процессе ремонта, Готова к выдаче

        [Display(Name = "Цвет")]
        [StringLength(7)]
        public string Color { get; set; } = "#808080";

        [Display(Name = "Порядок")]
        public int Order { get; set; }
    }
}