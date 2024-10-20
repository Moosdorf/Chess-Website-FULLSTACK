using DataLayer.Entities;
namespace ChessServer.Models;

public class ChessModel
{
    public int Id { get; set; }
    public User player1 { get; set; }
    public User player2 { get; set; }
    public bool onGoing { get; set; }
    public string winner { get; set; }
}
