using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DataLayer.Entities.Chess;
public enum GameType
{
    Bot,
    Multiplayer
}

public enum GameResult
{
    WhiteWin,
    BlackWin,
    Draw,
    Ongoing
}

public class ChessGame
{
    public int Id { get; set; }
    public GameType GameType { get; set; }

    public int WhiteId { get; set; }
    public string WhiteUsername { get; set; } 
    public User WhitePlayer { get; set; } = null!; // remove null when ready
    public int BlackId { get; set; }
    public string BlackUsername { get; set; }
    public User BlackPlayer { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Move> Moves { get; set; } = []; 
    public GameResult Result { get; set; } = GameResult.Ongoing;
}