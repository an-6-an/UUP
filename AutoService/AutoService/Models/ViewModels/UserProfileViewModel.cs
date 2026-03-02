using System.ComponentModel.DataAnnotations;

namespace AutoService.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите ФИО")]
        [Display(Name = "ФИО")]
        public string FIO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите телефон")]
        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        [Display(Name = "Тип пользователя")]
        public string UserType { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен быть от 4 символов")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }
    }
}