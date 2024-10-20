namespace DataLayer.Entities;

public class ChessGame
{
    public int chessId { get; set; }
    public virtual User[] players { get; set; }
    public List<ChessMove> moves = new List<ChessMove>();
}