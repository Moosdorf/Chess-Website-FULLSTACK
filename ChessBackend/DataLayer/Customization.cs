namespace DataLayer
{
    public class Customization
    {
        public string boardPref { get; set; } = string.Empty;
        public string piecePref { get; set; } = string.Empty;
        public bool darkMode { get; set; }
        public int volume { get; set; }
        public virtual User user { get; set; }
        public int userId { get; set; }
        public int customizationId { get; set; }

    }
}