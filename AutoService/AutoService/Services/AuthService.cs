using AutoService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Services
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyUserId = "UserId";
        private const string SessionKeyUserLogin = "UserLogin";
        private const string SessionKeyUserFIO = "UserFIO";
        private const string SessionKeyUserType = "UserType";

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Вход пользователя
        public void SignIn(User user)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetInt32(SessionKeyUserId, user.Id);
                session.SetString(SessionKeyUserLogin, user.Login);
                session.SetString(SessionKeyUserFIO, user.FIO);
                session.SetString(SessionKeyUserType, user.Type);
            }
        }

        // Выход пользователя
        public void SignOut()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Clear();
        }

        // Получение ID текущего пользователя
        public int? GetCurrentUserId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetInt32(SessionKeyUserId);
        }

        // Получение текущего пользователя
        public User? GetCurrentUser()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return null;

            var userId = session.GetInt32(SessionKeyUserId);
            if (!userId.HasValue) return null;

            return new User
            {
                Id = userId.Value,
                Login = session.GetString(SessionKeyUserLogin) ?? string.Empty,
                FIO = session.GetString(SessionKeyUserFIO) ?? string.Empty,
                Type = session.GetString(SessionKeyUserType) ?? string.Empty
            };
        }

        // Проверка, авторизован ли пользователь
        public bool IsAuthenticated()
        {
            return GetCurrentUserId().HasValue;
        }

        // Проверка роли
        public bool IsInRole(string role)
        {
            var userType = _httpContextAccessor.HttpContext?.Session?.GetString(SessionKeyUserType);
            return userType == role;
        }

        // Проверка, является ли пользователь менеджером
        public bool IsManager()
        {
            return IsInRole("Менеджер");
        }

        // Проверка, является ли пользователь механиком
        public bool IsMechanic()
        {
            return IsInRole("Автомеханик");
        }

        // Проверка, является ли пользователь заказчиком
        public bool IsClient()
        {
            return IsInRole("Заказчик");
        }

        // Проверка, является ли пользователь оператором
        public bool IsOperator()
        {
            return IsInRole("Оператор");
        }
    }
}