using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Rook(bool white) : Piece(white)
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
        ];

        foreach (var dir in directions)
        {
            bool kingHit = false;
            int dRow = dir[0], dCol = dir[1];
            for (int iRow = row + dRow, iCol = col + dCol;
                 iRow >= 0 && iRow < 8 && iCol >= 0 && iCol < 8;
                 iRow += dRow, iCol += dCol)
            {
                var target = chessState.GameBoard[iRow][iCol];
                if (kingHit)
                {
                    if (target.Type != PieceType.Empty) break;
                    target.Attackers.Add(Position);
                    continue;
                }
                if (!UpdateMoves(chessState, iRow, iCol))
                {
                    if (target.Type == PieceType.King && target.IsWhite != IsWhite)
                    {
                        kingHit = true;
                        continue;
                    }
                    break;
                }
            }
        }
    }

}
