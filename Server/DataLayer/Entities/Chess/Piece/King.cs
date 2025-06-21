using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess.Piece
{
    public class King(bool white) : Piece(white)
    {
        public override void FindMoves(Piece[][] board)
        {
            (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);


            for (int iRow = row - 1; iRow <= row + 1; iRow++)
            {
                for (int iCol = col - 1; iCol <= col + 1; iCol++)
                {
                    if (iRow >= 0 && iRow < 8 && iCol >= 0 && iCol < 8)
                    {
                        if (board[iRow][iCol] == this) continue;
                        CheckSquare(board, iRow, iCol);
                    }
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
}
