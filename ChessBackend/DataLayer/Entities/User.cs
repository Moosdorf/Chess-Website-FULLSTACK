namespace DataLayer.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public virtual List<UserChessGames> ChessGames { get; set; }

}