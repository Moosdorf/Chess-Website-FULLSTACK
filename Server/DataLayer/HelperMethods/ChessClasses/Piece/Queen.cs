using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Queen(bool white) : Piece(white)   
{
    public override void FindMoves(ChessInfo chessState)
    {
        (int row, int col) = ChessMethods.RankFileToRowCol(Position);

        int[][] directions =
        [
            [-1, 0], // up
            [1, 0],  // down
            [0, 1],   // right
            [0, -1],  // left
            [-1, -1], // up-left
            [1, -1],  // down-left
            [1, 1],   // down-right
            [-1, 1],  // up-right
        ];


        foreach (var dir in directions)
        {
            int dRow = dir[0], dCol = dir[1];
            for (int iRow = row + dRow, iCol = col + dCol;
                 iRow >= 0 && iRow < 8 && iCol >= 0 && iCol < 8; // within 
                 iRow += dRow, iCol += dCol)
            {
                if (!UpdateMoves(chessState, iRow, iCol)) break;
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
