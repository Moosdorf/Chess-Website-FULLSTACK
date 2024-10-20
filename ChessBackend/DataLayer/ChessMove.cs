namespace DataLayer;

public class ChessMove
{
    public int moveId { get; set; }
    public string move{ get; set; }
    public ChessGame game { get; set; }
    public int chessId { get; set; }
    public User player { get; set; }
    public int moveNumber { get; set; }
}