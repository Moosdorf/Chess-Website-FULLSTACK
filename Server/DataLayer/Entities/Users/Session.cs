namespace DataLayer.Entities.Users;

public class Session
{
    public int sessionId { get; set; }
    public virtual User user { get; set; }
    public int userId { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime? endedAt { get; set; }
}