using DataLayer.Entities.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.HelperMethods
{
    public static class ChessMethods
    {
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

        public static List<string> FindCheckBlockers(Piece[][] chessBoard, King king, Piece pieceChecked)
        {
            king.Check = true;
            var blockers = new List<string>() { pieceChecked.Position };
            switch (pieceChecked.Type)
                {
                    case PieceType.Queen:
                        blockers = blockers.Concat(DiagonalBlocks(chessBoard, king, pieceChecked)).ToList();
                        blockers = blockers.Concat(StraightBlocks(chessBoard, king, pieceChecked)).ToList();
                    break;

                    case PieceType.Rook:
                        blockers = blockers.Concat(StraightBlocks(chessBoard, king, pieceChecked)).ToList();
                    break;

                    case PieceType.Bishop:
                        blockers = blockers.Concat(DiagonalBlocks(chessBoard, king, pieceChecked)).ToList();
                    break;
            }

            return blockers;
        }

        private static List<string> DiagonalBlocks(Piece[][] chessBoard, King king, Piece pieceChecked)
        {
            var blockers = new List<string>();
            (int aRow, int aCol) = ChessMethods.RankFileToRowCol(pieceChecked.Position);
            (int kRow, int kCol) = ChessMethods.RankFileToRowCol(king.Position);
            // check if they are diagonal to each other
            if (Math.Abs(aRow - kRow) != Math.Abs(aCol - kCol)) return []; // return empty list

            var verticalDirection = (aRow > kRow) ? -1 : 1; // true = king to the left of piece
            var horizontalDirection = (aCol > kCol) ? -1 : 1; // true = king "above" piece
            var distance = (aRow > kRow) ? (aRow - kRow) - 1 : (kRow - aRow);
            Console.WriteLine("distance: " +  distance);

            for (int i = 1; i < distance; i++)
            {
                var square = chessBoard[aRow + i*verticalDirection][aCol + i*horizontalDirection];
                if (square != king && square.Type == PieceType.Empty)
                {
                    blockers.Add(square.Position);
                }
                else break;
            }

            // true true = up right
            // true false = up left
            // false true = down right
            // false false= down left

            return blockers;
        }
        private static List<string> StraightBlocks(Piece[][] chessBoard, King king, Piece pieceChecked)
        {
            var blockers = new List<string>();
            (int aRow, int aCol) = ChessMethods.RankFileToRowCol(pieceChecked.Position);
            (int kRow, int kCol) = ChessMethods.RankFileToRowCol(king.Position);


            return blockers;
        }


        public static bool ValidateMove(string move, Piece[][] board)
        {
            var (fRow, fCol, tRow, tCol) = ConvertMoveToColRow(move);
            var attacker = board[fRow][fCol];
            var victim = board[tRow][tCol];
            if (attacker == null || victim == null)
            {
                Console.WriteLine("someone null");
                return false;
            }
            if (!attacker.AvailableCaptures.Contains(victim.Position) && !attacker.AvailableMoves.Contains(victim.Position))
            {
                
                Console.WriteLine("does not contain in list - " + attacker.AvailableCaptures.Count + " " + attacker.AvailableMoves.Count);
                return false; 
            }

            return true;
        }
        public static Piece[][] findAvailableMoves(Piece[][] chessBoard)
        {

            foreach (var piece in chessBoard.SelectMany(row => row))
            {
                piece.AvailableMoves = new();
                piece.AvailableCaptures = new();
                piece.Attackers = new();
                piece.Defenders = new();
                piece.FindMoves(chessBoard);
            }
            return chessBoard;
        }

        public static (int fromRow, int fromCol, int toRow, int toCol) ConvertMoveToColRow(string move)
        {
            var fromTo = move.Split(',');

            var from = fromTo[0];
            var to = fromTo[1];

            (int tRow, int tCol) = RankFileToRowCol(to);
            (int fRow, int fCol) = RankFileToRowCol(from);
            return (fRow, fCol, tRow, tCol);
        }

        public static void MakeMove(Piece[][] chessBoard, string move)
        {

            var (fRow, fCol, tRow, tCol) = ConvertMoveToColRow(move);

            var target = chessBoard[tRow][tCol];
            var attacker = chessBoard[fRow][fCol];
            Console.WriteLine("attacker: " + attacker.Position);
            Console.WriteLine("target: " + target.Position);
            Console.WriteLine();

            chessBoard[tRow][tCol] = attacker;
            chessBoard[tRow][tCol].Position = RowColToRankFile(tRow, tCol);
            attacker.Moves++;
            if (target.Type != attacker.Type) attacker.Captures++;

            chessBoard[fRow][fCol] = new Empty(false) { Type = PieceType.Empty, Position = RowColToRankFile(fRow, fCol) };
        }
        public static Piece[][] CreateGameBoard()
        {
            var chessBoard = new Piece[8][]; // ends as final result 

            for (int row = 0; row < 8; row++) // initiate each row in the jagged array (must be done otherwise we have no arrays to push to)
            {
                chessBoard[row] = new Piece[8];
            }

            chessBoard[0][0] = new Rook(true) { Type = PieceType.Rook, Position = RowColToRankFile(0, 0) };
            chessBoard[0][1] = new Knight(true) { Type = PieceType.Knight, Position = RowColToRankFile(0, 1) };
            chessBoard[0][2] = new Bishop(true) { Type = PieceType.Bishop, Position = RowColToRankFile(0, 2) };
            chessBoard[0][3] = new Queen(true) { Type = PieceType.Queen, Position = RowColToRankFile(0, 3) };
            chessBoard[0][4] = new King(true) { Type = PieceType.King, Position = RowColToRankFile(0, 4) };
            chessBoard[0][5] = new Bishop(true) { Type = PieceType.Bishop, Position = RowColToRankFile(0, 5) };
            chessBoard[0][6] = new Knight(true) { Type = PieceType.Knight, Position = RowColToRankFile(0, 6) };
            chessBoard[0][7] = new Rook(true) { Type = PieceType.Rook, Position = RowColToRankFile(0, 7) };

            chessBoard[7][0] = new Rook(false) { Type = PieceType.Rook, Position = RowColToRankFile(7, 0) };
            chessBoard[7][1] = new Knight(false) { Type = PieceType.Knight, Position = RowColToRankFile(7, 1) };
            chessBoard[7][2] = new Bishop(false) { Type = PieceType.Bishop, Position = RowColToRankFile(7, 2) };
            chessBoard[7][3] = new Queen(false) { Type = PieceType.Queen, Position = RowColToRankFile(7, 3) };
            chessBoard[7][4] = new King(false) { Type = PieceType.King, Position = RowColToRankFile(7, 4) };
            chessBoard[7][5] = new Bishop(false) { Type = PieceType.Bishop, Position = RowColToRankFile(7, 5) };
            chessBoard[7][6] = new Knight(false) { Type = PieceType.Knight, Position = RowColToRankFile(7, 6) };
            chessBoard[7][7] = new Rook(false) { Type = PieceType.Rook, Position = RowColToRankFile(7, 7) };



            // creating and pushing black pawns
            for (int col = 0; col < 8; col++)
            {
                chessBoard[1][col] = new Pawn(true) { Type = PieceType.Pawn, Position = RowColToRankFile(1, col) }; // insert white pawns
                chessBoard[6][col] = new Pawn(false) { Type = PieceType.Pawn, Position = RowColToRankFile(6, col) }; // insert black pawns

            }

            for (int row = 2; row < 6; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    chessBoard[row][col] = new Empty(false) { Type = PieceType.Empty, Position = RowColToRankFile(row, col) };
                }
            }
            chessBoard = findAvailableMoves(chessBoard);

            return chessBoard;
        }
    }
}
