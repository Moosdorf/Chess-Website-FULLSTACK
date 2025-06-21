using DataLayer.Entities.Chess.Piece;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HelperMethods
{
    public static class ChessMethods
    {
        public enum PieceType
        {
            Rook, Knight, Bishop, Queen, King, Pawn, Empty
        }
        public static string RowColToRankFile(int row,
                                              int col)
        {
            char file = (char)(col + 97); // converting integer to char, just add 97 to find alphabet. 0 + 97 = 'a', 1 + 97 = 'b' and so on.
            int rank = row + 1; // create rank numbers (just increase by one)

            return $"{file}{rank}";
        }
        public static (int,int) RankFileToRowCol(string fileRank) // "e3" to 4,2
        {
            int row = fileRank[1] - 48 - 1; // '1' - 48 = 1. then to get index instead -1 again

            int col =  fileRank[0] - 97; // same as the other file converstion in RowCol, just reversed

            return (row, col);
        }


        public static bool checkMove(Piece[][] chessBoard, (int, int) move, (int, int) from)
        {
            bool whitesTurn = chessBoard[from.Item1][from.Item2].IsWhite;

            return false;
        }

        public static Piece[][] findAvailableMoves(Piece[][] chessBoard)
        {
            foreach (var row in chessBoard)
            {
                foreach (var piece in row)
                {
                    piece.AvailableMoves = new();
                    piece.AvailableCaptures = new();
                    piece.Attackers = new();
                    piece.Defenders = new();
                    piece.FindMoves(chessBoard);
                }
            }
            return chessBoard;
        }
        public static Piece[][] MakeMove(Piece[][] chessBoard, string move)
        {

            var fromTo = move.Split(',');

            var from = fromTo[0];
            var to = fromTo[1];

            (int tRow, int tCol) = RankFileToRowCol(to);
            (int fRow, int fCol) = RankFileToRowCol(from);
            var target = chessBoard[tRow][tCol];
            chessBoard[tRow][tCol] = chessBoard[fRow][fCol];
            chessBoard[tRow][tCol].Position = RowColToRankFile(tRow, tCol);

            if (target.Type != "empty") chessBoard[tRow][tCol].Captures++;
            chessBoard[tRow][tCol].Moves++;
            chessBoard[fRow][fCol] = new Empty(false) { Type = "empty", Position = RowColToRankFile(fRow, fCol) };
            chessBoard = ChessMethods.findAvailableMoves(chessBoard);

            return chessBoard;
        }
        public static Piece[][] CreateGameBoard()
        {
            var chessBoard = new Piece[8][]; // ends as final result 

            for (int row = 0; row < 8; row++) // initiate each row in the jagged array (must be done otherwise we have no arrays to push to)
            {
                chessBoard[row] = new Piece[8];
            }

            chessBoard[0][0] = new Rook(true) { Type = "rook", Position = RowColToRankFile(0, 0) };
            chessBoard[0][1] = new Knight(true) { Type = "knight", Position = RowColToRankFile(0, 1) };
            chessBoard[0][2] = new Bishop(true) { Type = "bishop", Position = RowColToRankFile(0, 2) };
            chessBoard[0][3] = new Queen(true) { Type = "queen", Position = RowColToRankFile(0, 3) };
            chessBoard[0][4] = new King(true) { Type = "king", Position = RowColToRankFile(0, 4) };
            chessBoard[0][5] = new Bishop(true) { Type = "bishop", Position = RowColToRankFile(0, 5) };
            chessBoard[0][6] = new Knight(true) { Type = "knight", Position = RowColToRankFile(0, 6) };
            chessBoard[0][7] = new Rook(true) { Type = "rook", Position = RowColToRankFile(0, 7) };

            chessBoard[7][0] = new Rook(false) { Type = "rook", Position = RowColToRankFile(7, 0) };
            chessBoard[7][1] = new Knight(false) { Type = "knight", Position = RowColToRankFile(7, 1) };
            chessBoard[7][2] = new Bishop(false) { Type = "bishop", Position = RowColToRankFile(7, 2) };
            chessBoard[7][3] = new Queen(false) { Type = "queen", Position = RowColToRankFile(7, 3) };
            chessBoard[7][4] = new King(false) { Type = "king", Position = RowColToRankFile(7, 4) };
            chessBoard[7][5] = new Bishop(false) { Type = "bishop", Position = RowColToRankFile(7, 5) };
            chessBoard[7][6] = new Knight(false) { Type = "knight", Position = RowColToRankFile(7, 6) };
            chessBoard[7][7] = new Rook(false) { Type = "rook", Position = RowColToRankFile(7, 7) };



            // creating and pushing black pawns
            for (int col = 0; col < 8; col++)
            {
                chessBoard[1][col] = new Pawn(true) { Type = "pawn", Position = RowColToRankFile(1, col) }; // insert white pawns
                chessBoard[6][col] = new Pawn(false) { Type = "pawn", Position = RowColToRankFile(6, col) }; // insert black pawns

            }

            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    chessBoard[row][col] = new Empty(false) { Type = "empty", Position = RowColToRankFile(row, col) };
                }
            }
            chessBoard = findAvailableMoves(chessBoard);
            return chessBoard;
        }
    }
}
