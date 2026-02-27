using AutoService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            // Проверяем, есть ли уже данные
            if (await context.Users.AnyAsync())
            {
                return; // БД уже заполнена
            }

            // Создаем статусы
            var statuses = new Status[]
            {
                new Status { Name = "Новая заявка", Color = "#17a2b8", Order = 1 },
                new Status { Name = "В процессе ремонта", Color = "#ffc107", Order = 2 },
                new Status { Name = "Готова к выдаче", Color = "#28a745", Order = 3 },
                new Status { Name = "Отменена", Color = "#dc3545", Order = 4 }
            };
            await context.Statuses.AddRangeAsync(statuses);
            await context.SaveChangesAsync();

            // Создаем пользователей
            var users = new User[]
            {
                new User { FIO = "Белов Александр Давидович", Phone = "89210563128", Login = "login1", Password = "pass1", Type = "Менеджер" },
                new User { FIO = "Харитонова Мария Павловна", Phone = "89535078985", Login = "login2", Password = "pass2", Type = "Автомеханик" },
                new User { FIO = "Марков Давид Иванович", Phone = "89210673849", Login = "login3", Password = "pass3", Type = "Автомеханик" },
                new User { FIO = "Громова Анна Семёновна", Phone = "89990563748", Login = "login4", Password = "pass4", Type = "Оператор" },
                new User { FIO = "Карташова Мария Данииловна", Phone = "89994563847", Login = "login5", Password = "pass5", Type = "Оператор" },
                new User { FIO = "Касаткин Егор Львович", Phone = "89219567849", Login = "login11", Password = "pass11", Type = "Заказчик" },
                new User { FIO = "Ильина Тамара Даниловна", Phone = "89219567841", Login = "login12", Password = "pass12", Type = "Заказчик" },
                new User { FIO = "Елисеева Юлиана Алексеевна", Phone = "89219567842", Login = "login13", Password = "pass13", Type = "Заказчик" },
                new User { FIO = "Никифорова Алиса Тимофеевна", Phone = "89219567843", Login = "login14", Password = "pass14", Type = "Заказчик" },
                new User { FIO = "Васильев Али Евгеньевич", Phone = "89219567844", Login = "login15", Password = "pass15", Type = "Автомеханик" }
            };
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Создаем типы автомобилей
            var carTypes = new CarType[]
            {
                new CarType { Name = "Легковая" },
                new CarType { Name = "Грузовая" }
            };
            await context.CarTypes.AddRangeAsync(carTypes);
            await context.SaveChangesAsync();

            // Создаем автомобили
            var cars = new Car[]
            {
    new Car { CarTypeId = 1, CarModel = "Hyundai Avante (CN7)" },
    new Car { CarTypeId = 1, CarModel = "Nissan 180SX" },
    new Car { CarTypeId = 1, CarModel = "Toyota 2000GT" },
    new Car { CarTypeId = 2, CarModel = "Citroen Berlingo (B9)" },
    new Car { CarTypeId = 2, CarModel = "УАЗ 2360" }
            };
            await context.Cars.AddRangeAsync(cars);
            await context.SaveChangesAsync();

            // Создаем заявки
            var requests = new Request[]
            {
                new Request {
                    StartDate = new DateTime(2023, 6, 6),
                    CarId = 1,
                    ProblemDescription = "Отказали тормоза.",
                    RequestStatus = "В процессе ремонта",
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = 2,
                    ClientId = 7,
                    CreatedAt = DateTime.Now
                },
                new Request {
                    StartDate = new DateTime(2023, 5, 5),
                    CarId = 2,
                    ProblemDescription = "Отказали тормоза.",
                    RequestStatus = "В процессе ремонта",
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = 3,
                    ClientId = 8,
                    CreatedAt = DateTime.Now
                },
                new Request {
                    StartDate = new DateTime(2022, 7, 7),
                    CarId = 3,
                    ProblemDescription = "В салоне пахнет бензином.",
                    RequestStatus = "Готова к выдаче",
                    CompletionDate = new DateTime(2023, 1, 1),
                    RepairParts = null,
                    MasterId = 3,
                    ClientId = 9,
                    CreatedAt = DateTime.Now
                },
                new Request {
                    StartDate = new DateTime(2023, 8, 2),
                    CarId = 4,
                    ProblemDescription = "Руль плохо крутится.",
                    RequestStatus = "Новая заявка",
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = null,
                    ClientId = 8,
                    CreatedAt = DateTime.Now
                },
                new Request {
                    StartDate = new DateTime(2023, 8, 2),
                    CarId = 5,
                    ProblemDescription = "Руль плохо крутится.",
                    RequestStatus = "Новая заявка",
                    CompletionDate = null,
                    RepairParts = null,
                    MasterId = null,
                    ClientId = 9,
                    CreatedAt = DateTime.Now
                }
            };
            await context.Requests.AddRangeAsync(requests);
            await context.SaveChangesAsync();

            // Создаем комментарии
            var comments = new Comment[]
            {
                new Comment { Message = "Очень странно.", MasterId = 2, RequestId = 1, CreatedAt = DateTime.Now },
                new Comment { Message = "Будем разбираться!", MasterId = 3, RequestId = 2, CreatedAt = DateTime.Now },
                new Comment { Message = "Будем разбираться!", MasterId = 3, RequestId = 3, CreatedAt = DateTime.Now }
            };
            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();
        }
    }
}