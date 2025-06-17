using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess.Piece
{
    public class Pawn(bool white) : Piece(white)
    {
        public override void FindMoves(Piece[][] board)
        {
            (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);
            Piece piece;
            // moving 
            if (this.Moves == 0) // can move two squares
            {
                for (int i = 1; i < 3; i++)
                {
                    piece = board[(IsWhite) ? row + i : row - i][col]; // check two in front
                    if (piece.Type == "empty") AvailableMoves.Add(piece.Position);
                    else break;
                }
            } else
            {
                if (row + 1 < 8 && row - 1 >= 0)
                {
                    piece = board[(IsWhite) ? row + 1 : row - 1][col]; // check only one in front
                    if (piece.Type == "empty") AvailableMoves.Add(piece.Position);
                }

            }

            // captures left
            if (col - 1 >= 0 && row + 1 < 8 && row - 1 >= 0)
            {
                piece = board[(IsWhite) ? row + 1 : row - 1][col - 1]; // check left-side capture, if the piece is not at the edge
                if (piece.Type != "empty" && piece.IsWhite != IsWhite) AvailableMoves.Add(piece.Position);
            }

            // captures right
            if (col + 1 <= 7 && row + 1 < 8 && row - 1 >= 0)
            {
                piece = board[(IsWhite) ? row + 1 : row - 1][col + 1]; // same but for right-side
                if (piece.Type != "empty" && piece.IsWhite != IsWhite) AvailableMoves.Add(piece.Position);
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
