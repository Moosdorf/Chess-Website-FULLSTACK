using DataLayer.Entities;
using DataLayer.Models.User;
namespace DataLayer.Models.Chess;

public class ChessModel
{
    public int Id { get; set; }
    public UserModel player1 { get; set; }
    public UserModel player2 { get; set; }
    public bool onGoing { get; set; }
    public string winner { get; set; }
}
