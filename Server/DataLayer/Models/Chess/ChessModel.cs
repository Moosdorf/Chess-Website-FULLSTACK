using DataLayer.Entities;
using DataLayer.Entities.Chess.Piece;
using DataLayer.Models.User;
namespace DataLayer.Models.Chess;

public class ChessModel
{
    public int Id { get; set; }
    public Piece[][] Chessboard { get; set; } = null!;
    public int Moves { get; set;}
    public bool IsWhite { get; set; }

}
