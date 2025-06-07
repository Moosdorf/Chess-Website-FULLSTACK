namespace DataLayer.Entities.Chess;

public class ChessGame
{
    public int ChessId { get; set; }
    public List<ChessGameMoves> moves = new List<ChessGameMoves>();
}