using DataLayer.Entities.Chess;
using System.ComponentModel.DataAnnotations;
namespace DataLayer.Entities.Users;
public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password {  get; set; }
    public string Salt {  get; set; }
    public string Email {  get; set; } = string.Empty;


    // need both in order to link players
    public List<ChessGame> WhiteGames { get; set; } = [];
    public List<ChessGame> BlackGames { get; set; } = [];

}