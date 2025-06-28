using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Knight(bool white) : Piece(white)
{
    public override void FindMoves(ChessInfo chessState)
    {
        (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);
        
        var offsets = new (int dRow, int dCol)[]
        {
            (-2, -1), (-2, 1),
            (-1, -2), (-1, 2),
            (1, -2),  (1, 2),
            (2, -1),  (2, 1),
        };

        foreach (var (dRow, dCol) in offsets)
        {
            int targetRow = row + dRow;
            int targetCol = col + dCol;

            if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
            {
                UpdateMoves(chessState, targetRow, targetCol);
            }
        }
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
