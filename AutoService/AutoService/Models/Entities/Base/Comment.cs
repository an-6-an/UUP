using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        [Required]
        [Display(Name = "Сообщение")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty; // Вместо Content

        [Required]
        [Display(Name = "Мастер")]
        public int MasterId { get; set; }

        [Required]
        [Display(Name = "Заявка")]
        public int RequestId { get; set; }

        // Навигационные свойства
        [ForeignKey("MasterId")]
        public virtual User Master { get; set; } = null!;

        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; } = null!;

        [NotMapped]
        public string MasterName => Master?.FIO ?? "Неизвестно";
    }
}