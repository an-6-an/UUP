using System.ComponentModel.DataAnnotations;

namespace AutoService.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите ФИО")]
        [Display(Name = "ФИО")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "ФИО должно быть от 5 до 100 символов")]
        public string FIO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите телефон")]
        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [RegularExpression(@"^8\d{10}$", ErrorMessage = "Телефон должен быть в формате 8XXXXXXXXXX")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 50 символов")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Пароль должен быть от 4 символов")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите тип пользователя")]
        [Display(Name = "Тип пользователя")]
        public string UserType { get; set; } = "Заказчик";
    }
}