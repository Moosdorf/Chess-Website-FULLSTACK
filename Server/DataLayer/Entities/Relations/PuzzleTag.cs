using DataLayer.Entities.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Relations
{
    public class PuzzleTag
    {
        public string PuzzleId { get; set; }
        public Puzzle Puzzle { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
