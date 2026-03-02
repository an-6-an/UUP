using AutoService.Models;

namespace AutoService.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any()) return; // уже заполнено

        var users = new List<User>
        {
            new() { UserID = 1,  FIO = "Белов Александр Давидович",    Phone = "89210563128", Login = "login1",  Password = "pass1",  Type = "Менеджер" },
            new() { UserID = 2,  FIO = "Харитонова Мария Павловна",     Phone = "89535078985", Login = "login2",  Password = "pass2",  Type = "Автомеханик" },
            new() { UserID = 3,  FIO = "Марков Давид Иванович",         Phone = "89210673849", Login = "login3",  Password = "pass3",  Type = "Автомеханик" },
            new() { UserID = 4,  FIO = "Громова Анна Семёновна",        Phone = "89990563748", Login = "login4",  Password = "pass4",  Type = "Оператор" },
            new() { UserID = 5,  FIO = "Карташова Мария Данииловна",    Phone = "89994563847", Login = "login5",  Password = "pass5",  Type = "Оператор" },
            new() { UserID = 6,  FIO = "Касаткин Егор Львович",         Phone = "89219567849", Login = "login11", Password = "pass11", Type = "Заказчик" },
            new() { UserID = 7,  FIO = "Ильина Тамара Даниловна",       Phone = "89219567841", Login = "login12", Password = "pass12", Type = "Заказчик" },
            new() { UserID = 8,  FIO = "Елисеева Юлиана Алексеевна",    Phone = "89219567842", Login = "login13", Password = "pass13", Type = "Заказчик" },
            new() { UserID = 9,  FIO = "Никифорова Алиса Тимофеевна",   Phone = "89219567843", Login = "login14", Password = "pass14", Type = "Заказчик" },
            new() { UserID = 10, FIO = "Васильев Али Евгеньевич",       Phone = "89219567844", Login = "login15", Password = "pass15", Type = "Автомеханик" },
        };
        db.Users.AddRange(users);
        db.SaveChanges();

        var requests = new List<Request>
        {
            new() { RequestID = 1, StartDate = new DateTime(2023,6,6),  CarType = "Легковая", CarModel = "Hyundai Avante (CN7)",  ProblemDescription = "Отказали тормоза.",        RequestStatus = "В процессе ремонта", MasterID = 2, ClientID = 7 },
            new() { RequestID = 2, StartDate = new DateTime(2023,5,5),  CarType = "Легковая", CarModel = "Nissan 180SX",          ProblemDescription = "Отказали тормоза.",        RequestStatus = "В процессе ремонта", MasterID = 3, ClientID = 8 },
            new() { RequestID = 3, StartDate = new DateTime(2022,7,7),  CarType = "Легковая", CarModel = "Toyota 2000GT",         ProblemDescription = "В салоне пахнет бензином.",RequestStatus = "Готова к выдаче",    CompletionDate = new DateTime(2023,1,1), MasterID = 3, ClientID = 9 },
            new() { RequestID = 4, StartDate = new DateTime(2023,8,2),  CarType = "Грузовая", CarModel = "Citroen Berlingo (B9)", ProblemDescription = "Руль плохо крутится.",     RequestStatus = "Новая заявка",       ClientID = 8 },
            new() { RequestID = 5, StartDate = new DateTime(2023,8,2),  CarType = "Грузовая", CarModel = "УАЗ 2360",             ProblemDescription = "Руль плохо крутится.",     RequestStatus = "Новая заявка",       ClientID = 9 },
        };
        db.Requests.AddRange(requests);
        db.SaveChanges();

        var comments = new List<Comment>
        {
            new() { CommentID = 1, Message = "Очень странно.",       MasterID = 2, RequestID = 1 },
            new() { CommentID = 2, Message = "Будем разбираться!",   MasterID = 3, RequestID = 2 },
            new() { CommentID = 3, Message = "Будем разбираться!",   MasterID = 3, RequestID = 3 },
        };
        db.Comments.AddRange(comments);
        db.SaveChanges();
    }
}
