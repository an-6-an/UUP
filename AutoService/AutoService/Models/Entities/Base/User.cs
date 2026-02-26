using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Models.Entities.Base;

namespace AutoService.Models.Entities
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required]
        [Display(Name = "ФИО")]
        [StringLength(100)]
        public string FIO { get; set; } = string.Empty; // Вместо FirstName + LastName

        [Required]
        [Display(Name = "Телефон")]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Логин")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Тип пользователя")]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty; // Менеджер, Автомеханик, Оператор, Заказчик

        // Навигационные свойства
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>(); // как клиент
        public virtual ICollection<Request> MasterRequests { get; set; } = new List<Request>(); // как мастер
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}