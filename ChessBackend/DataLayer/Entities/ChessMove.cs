namespace DataLayer.Entities;

public class ChessMove
{
    public int moveId { get; set; }
    public string move { get; set; }
    public ChessGame game { get; set; }
    public User player { get; set; }
    public int moveNumber { get; set; }
}