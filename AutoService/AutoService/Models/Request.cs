namespace AutoService.Models;

public class Request
{
    public int RequestID { get; set; }
    public DateTime StartDate { get; set; }
    public string CarType { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public string ProblemDescription { get; set; } = string.Empty;
    public string RequestStatus { get; set; } = "Новая заявка";
    public DateTime? CompletionDate { get; set; }
    public string? RepairParts { get; set; }

    public int? MasterID { get; set; }
    public User? Master { get; set; }

    public int ClientID { get; set; }
    public User Client { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
