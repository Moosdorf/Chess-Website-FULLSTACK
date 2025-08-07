using DataLayer.Entities.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Chess
{
    public class ChessGameHistoryDTO
    {
        public int Id { get; set; }
        public string WhitePlayer { get; set; }
        public string BlackPlayer { get; set; }
        public string Winner { get; set; }
        public List<Move> Moves { get; set; }
        public float Length { get; set; }
        public string FEN { get; set; }
    }
}
