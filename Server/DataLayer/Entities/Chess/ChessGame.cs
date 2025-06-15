using DataLayer.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Chess;

public class ChessGame
{
    public int Id { get; set; }

    [Required]
    public User User1 { get; set; } // white

    [Required]
    public User User2 { get; set; } // black

    public List<Move> moves = [];
}