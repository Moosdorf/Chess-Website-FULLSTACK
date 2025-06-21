using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess.Piece
{
    public class Bishop(bool white) : Piece(white)
    {
        public override void FindMoves(Piece[][] board)
        {
            (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);


            // up left
            for (int iRow = row - 1, iCol = col - 1; iRow >= 0 && iCol >= 0; iRow--, iCol--)
            {
                if (iRow >= 0 && iCol >= 0)
                {
                    bool flowControl = CheckSquare(board, iRow, iCol);
                    if (!flowControl)
                    {
                        break;
                    }
                }
            }

            // down left
            for (int iRow = row + 1, iCol = col - 1; iRow < 8 && iCol >= 0; iRow++, iCol--)
            {
                if (iRow < 8 && iCol >= 0)
                {
                    bool flowControl = CheckSquare(board, iRow, iCol);
                    if (!flowControl)
                    {
                        break;
                    }
                }
            }

            // down right
            for (int iRow = row + 1, iCol = col + 1; iRow < 8 && iCol < 8; iRow++, iCol++)
            {
                if (iRow < 8 && iCol < 8)
                {
                    bool flowControl = CheckSquare(board, iRow, iCol);
                    if (!flowControl)
                    {
                        break;
                    }
                }
            }

            // up right
            for (int iRow = row - 1, iCol = col + 1; iRow >= 0 && iCol < 8; iRow--, iCol++)
            {
                if (iRow >= 0 && iCol < 8)
                {
                    bool flowControl = CheckSquare(board, iRow, iCol);
                    if (!flowControl)
                    {
                        break;
                    }
                }
            }
        }

        private bool CheckSquare(Piece[][] board, int iRow, int iCol)
        {
            var piece = board[iRow][iCol];
            if (piece.Type == "empty") AddMove(piece);
            else if (piece.IsWhite != this.IsWhite)
            {
                AddCaptures(piece);
                return false;
            }
            else return false; // piece is not empty and not enemy
            return true;
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
