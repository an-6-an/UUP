namespace AutoService.Models;

public class Comment
{
    public int CommentID { get; set; }
    public string Message { get; set; } = string.Empty;

    public int MasterID { get; set; }
    public User Master { get; set; } = null!;

    public int RequestID { get; set; }
    public Request Request { get; set; } = null!;
}
