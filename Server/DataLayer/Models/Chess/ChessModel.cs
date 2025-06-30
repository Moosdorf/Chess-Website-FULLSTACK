using DataLayer.Entities;
using DataLayer.Models.User;
namespace DataLayer.Models.Chess;

public class ChessModel
{
    public int Id { get; set; }
    public Piece[][] Chessboard { get; set; } = null!;
    public bool IsWhite { get; set; }
    public bool Check { get; set; } = false;
    public bool CheckMate { get; set; } = false;
    public List<string> BlockCheckPositions { get; set; } = [];
    public string FEN { get; set; } = "";

}
