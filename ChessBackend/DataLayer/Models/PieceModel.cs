namespace ChessServer.Models;

public class PieceModel
{
    private static int tempId = 0;
    public int id { get; } = ++tempId;
    public string piece { get; set; } = "blank";
    public string color { get; set; } = "blank";
    public int? row { get; set; }
    public int? col { get; set; }
    public int moves { get; set; } = 0;


    public List<int[]> availableMoves { get; set; } = new List<int[]> { new int[] { 1, 1 } };

    public override string? ToString()
    {
        return $"ID: {id}, Piece: {piece}, Color: {color}, Position: ({row ?? -1}, {col ?? -1}), Moves: {moves}, Available Moves: {string.Join(" | ", availableMoves.Select(m => $"[{string.Join(", ", m)}]"))}";
    }
}
