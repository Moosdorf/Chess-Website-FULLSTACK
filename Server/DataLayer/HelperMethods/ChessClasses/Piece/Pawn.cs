using DataLayer.HelperMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Pawn(bool white) : Piece(white) 
{
    public override void FindMoves(ChessInfo chessState)
    {
        (int row, int col) = ChessMethods.RankFileToRowCol(this.Position);

        // moving 
        MovePawnOneOrTwo(chessState, row, col);

        // captures left
        CaptureLeft(chessState, row, col);

        // captures right
        CaptureRight(chessState, row, col);
    }

    private void MovePawnOneOrTwo(ChessInfo chessState, int row, int col)
    {
        Piece piece;
        if (this.Moves == 0) // can move two squares
        {
            for (int i = 1; i < 3; i++)
            {
                piece = chessState.GameBoard[this.IsWhite ? row + i : row - i][col]; // check two in front
                if (piece.Type == PieceType.Empty)
                {
                    AddMove(chessState, piece);
                }
                else break;
            }
        }
        else
        {
            if (row + 1 < 8 && row - 1 >= 0)
            {
                piece = chessState.GameBoard[this.IsWhite ? row + 1 : row - 1][col]; // check only one in front
                if (piece.Type == PieceType.Empty)
                {
                    AddMove(chessState, piece);
                }
            }
        }
    }


    private void CaptureLeft(ChessInfo chessState, int row, int col)
    {
        if (col - 1 >= 0 && row + 1 < 8 && row - 1 >= 0)
        {
            var piece = chessState.GameBoard[this.IsWhite ? row + 1 : row - 1][col - 1]; // check left-side capture, if the piece is not at the edge
            AddCaptures(chessState, piece);
        }
    }



    private void CaptureRight(ChessInfo chessState, int row, int col)
    {
        if (col + 1 <= 7 && row + 1 <= 7 && row - 1 >= 0)
        {
            var piece = chessState.GameBoard[this.IsWhite ? row + 1 : row - 1][col + 1]; // same but for right-side
            AddCaptures(chessState, piece);
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

