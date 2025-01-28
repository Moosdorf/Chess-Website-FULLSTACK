namespace DataLayer.Entities;

public class ChessGame
{
    public int ChessId { get; set; }
    public virtual List<UserChessGames> Players { get; set; }
    public List<ChessGameMoves> moves = new List<ChessGameMoves>();
}