using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("Requests")]
    public class Request : BaseEntity
    {
        [Required]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        // CarType и CarModel теперь связаны через Car
        [Required]
        [Display(Name = "Автомобиль")]
        public int CarId { get; set; }

        [Required]
        [Display(Name = "Описание проблемы")]
        [DataType(DataType.MultilineText)]
        public string ProblemDescription { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Статус")]
        [StringLength(50)]
        public string RequestStatus { get; set; } = string.Empty; // Новая заявка, В процессе ремонта, Готова к выдаче

        [Display(Name = "Дата завершения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Запчасти")]
        [DataType(DataType.MultilineText)]
        public string? RepairParts { get; set; }

        [Display(Name = "Мастер")]
        public int? MasterId { get; set; } // masterID в ваших данных

        [Required]
        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        // Навигационные свойства
        [ForeignKey("CarId")]
        public virtual Car Car { get; set; } = null!;

        [ForeignKey("MasterId")]
        public virtual User? Master { get; set; }

        [ForeignKey("ClientId")]
        public virtual User Client { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Вычисляемые поля для удобства
        [NotMapped]
        public string CarInfo => $"{Car?.CarType?.Name} {Car?.CarModel}";

        [NotMapped]
        public string MasterName => Master?.FIO ?? "Не назначен";

        [NotMapped]
        public string ClientName => Client?.FIO ?? "Неизвестно";
    }
}