namespace AutoService.Models;

public class User
{
    public int UserID { get; set; }
    public string FIO { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Менеджер, Автомеханик, Оператор, Заказчик

    public ICollection<Request> RequestsAsClient { get; set; } = new List<Request>();
    public ICollection<Request> RequestsAsMaster { get; set; } = new List<Request>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
