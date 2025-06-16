using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess.Piece
{
    public class Queen(bool white) : Piece(white)   
    {
        public override void FindMoves(Piece[][] board)
        {
        }

        public override bool Capture()
        {
            throw new NotImplementedException();
        }

        public override bool Move()
        {
            throw new NotImplementedException();
        }
    }
}
