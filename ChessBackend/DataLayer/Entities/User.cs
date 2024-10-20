namespace DataLayer.Entities;

public class User
{
    public int userId { get; set; }
    public string username { get; set; } = string.Empty;
    public virtual List<ChessGame> chessGames { get; set; }
    public virtual Customization customization { get; set; }
    public virtual List<Session> sessions { get; set; }
}