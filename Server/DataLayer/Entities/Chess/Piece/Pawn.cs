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
                for (int i = 1; i<3; i++)
                {
                    piece = board[(IsWhite) ? row + i : row - i][col];
                    if (piece.Type == "empty") AvailableMoves.Add(piece.Position);
                }
            } else
            {
                piece = board[(IsWhite) ? row + 1 : row - 1][col];
                if (piece.Type == "empty") AvailableMoves.Add(piece.Position);
            }

            // captures left
            if (col - 1 >= 0)
            {
                piece = board[(IsWhite) ? row + 1 : row - 1][col - 1];
                if (piece.Type != "empty") AvailableMoves.Add(piece.Position);
            }

            // captures right
            if (col + 1 <= 7)
            {
                piece = board[(IsWhite) ? row + 1 : row - 1][col + 1];
                if (piece.Type != "empty") AvailableMoves.Add(piece.Position);
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
