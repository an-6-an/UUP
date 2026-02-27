using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("CarTypes")]
    public class CarType : BaseEntity
    {
        [Required]
        [Display(Name = "Тип автомобиля")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // Легковая, Грузовая и т.д.

        // Добавляем навигационное свойство
        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}