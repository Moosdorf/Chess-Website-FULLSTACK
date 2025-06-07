using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess
{
    public class ChessGameMoves
    {
        public int MoveId { get; set; }
        public int ChessId { get; set; }

        public ChessGame ChessGame { get; set; }
    }
}
