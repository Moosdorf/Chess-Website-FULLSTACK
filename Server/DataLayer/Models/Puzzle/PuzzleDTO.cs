using DataLayer.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Puzzle
{
    public class PuzzleDTO
    {
        public string PuzzleId { get; set; }
        public ChessInfo Chessboard { get; set; } = null!;
        public string FEN { get; set; }
        public int Rating { get; set; }
        public int RatingDeviation { get; set; }
        public int Popularity { get; set; }
        public int NbPlays { get; set; }
        public string GameUrl { get; set; }
        public List<string> OpeningTags { get; set; }
        public List<string> Moves { get; set; }
        public ICollection<TagDTO> Tags { get; set; } = new List<TagDTO>();
        public ICollection<PuzzleTag> PuzzleTags { get; set; } = new List<PuzzleTag>();
    }
}
public class TagDTO
{
    public string Name { get; set; }

}
