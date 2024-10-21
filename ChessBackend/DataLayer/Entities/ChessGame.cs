namespace DataLayer.Entities;

public class ChessGame
{
    public int chessId { get; set; }
    public virtual List<UserChessGames> Players { get; set; }
    public List<ChessMove> moves = new List<ChessMove>();
}