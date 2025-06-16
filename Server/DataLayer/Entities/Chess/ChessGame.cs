using DataLayer.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Chess;

public class ChessGame
{
    public int Id { get; set; }
    public List<Move> Moves { get; set; } = new();
}