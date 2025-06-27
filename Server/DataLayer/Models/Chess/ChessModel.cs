using DataLayer.Entities;
using DataLayer.Models.User;
namespace DataLayer.Models.Chess;

public class ChessModel
{
    public int Id { get; set; }
    public Piece[][] Chessboard { get; set; } = null!;
    public int Moves { get; set; } = 0;
    public bool IsWhite { get; set; }
    public bool Check { get; set; } = false;
    public List<string> BlockCheckPositions { get; set; } = [];

}
