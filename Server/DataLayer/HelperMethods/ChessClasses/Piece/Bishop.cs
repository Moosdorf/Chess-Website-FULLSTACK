using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Bishop(bool white) : Piece(white)
{
    public override void FindMoves(ChessInfo chessState)
    {
        (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);


        int[][] directions =
        [
            [-1, -1], // up-left
            [1, -1],  // down-left
            [1, 1],   // down-right
            [-1, 1],  // up-right
        ];

        foreach (var dir in directions)
        {
            int dRow = dir[0], dCol = dir[1];
            for (int iRow = row + dRow, iCol = col + dCol;
                 iRow >= 0 && iRow < 8 && iCol >= 0 && iCol < 8;
                 iRow += dRow, iCol += dCol)
            {
                if (!UpdateMoves(chessState, iRow, iCol)) break; // will return true if the direction is not blocked by a piece
            }
        }
    }

}
