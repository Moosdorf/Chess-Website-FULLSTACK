namespace DataLayer.Models.Chess
{
    public class MoveModel
    {
        public string Move { get; set; }
        public string? Promotion { get; set; } = null;
    }
}
