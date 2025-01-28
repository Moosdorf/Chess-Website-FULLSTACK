namespace ChessServer.Models;

public class PieceModel
{
    static int tempId = 0;
    public string color { get; set; } = "blank";
    public string piece { get; set; } = "blank";
    public int id { get; set; } = ++tempId;
}
