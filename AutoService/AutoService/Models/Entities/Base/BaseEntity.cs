using System.ComponentModel.DataAnnotations;

namespace AutoService.Models.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Дата обновления")]
        public DateTime? UpdatedAt { get; set; }
    }
}