using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Empty(bool white) : Piece(white)
{
    public override void FindMoves(ChessInfo chessState)
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
