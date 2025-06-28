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

        public static void FindCheckBlockers(ChessInfo chessState, King king, Piece pieceChecked)
        {
            var blockers = new List<string>() { pieceChecked.Position };
            switch (pieceChecked.Type)
                {
                    case PieceType.Queen:
                        blockers = blockers.Concat(DiagonalBlocks(chessState.GameBoard, king, pieceChecked)).ToList();
                        blockers = blockers.Concat(StraightBlocks(chessState.GameBoard, king, pieceChecked)).ToList();
                    break;

                    case PieceType.Rook:
                        blockers = blockers.Concat(StraightBlocks(chessState.GameBoard, king, pieceChecked)).ToList();
                    break;

                    case PieceType.Bishop:
                        blockers = blockers.Concat(DiagonalBlocks(chessState.GameBoard, king, pieceChecked)).ToList();
                    break;
            }

            chessState.Blockers = chessState.Blockers.Concat(blockers).ToList();
        }

        private static List<string> DiagonalBlocks(Piece[][] chessBoard, King king, Piece pieceChecked)
        {
            var blockers = new List<string>();
            (int aRow, int aCol) = ChessMethods.RankFileToRowCol(pieceChecked.Position);
            (int kRow, int kCol) = ChessMethods.RankFileToRowCol(king.Position);
            // check if they are diagonal to each other
            if (Math.Abs(aRow - kRow) != Math.Abs(aCol - kCol)) return blockers; // return empty list

            var verticalDirection = (aRow > kRow) ? -1 : 1; // true = king to the left of piece
            var horizontalDirection = (aCol > kCol) ? -1 : 1; // true = king "above" piece
            int distance = Math.Abs(aRow - kRow);
            Console.WriteLine("distance: " +  distance);

            for (int i = 1; i < distance; i++)
            {
                var square = chessBoard[aRow + i * verticalDirection][aCol + i * horizontalDirection];
                if (square.Type != PieceType.Empty) break;
                blockers.Add(square.Position);
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

            int deltaRow = 0, deltaCol = 0;
            int distance;

            if (aRow == kRow) // horizontal
            {
                distance = Math.Abs(aCol - kCol);
                deltaCol = (aCol > kCol) ? -1 : 1;
            }
            else if (aCol == kCol) // vertical
            {
                distance = Math.Abs(aRow - kRow);
                deltaRow = (aRow > kRow) ? -1 : 1;
            }
            else
            {
                return blockers; // Not in a straight line
            }

            for (int i = 1; i < distance; i++)
            {
                var square = chessBoard[aRow + i * deltaRow][aCol + i * deltaCol];
                if (square.Type != PieceType.Empty) break;
                blockers.Add(square.Position);
            }

            return blockers;
        }


        public static bool ValidateMove(string move, Piece[][] board)
        {
            var (fRow, fCol, tRow, tCol) = ConvertMoveToColRow(move);
            var attacker = board[fRow][fCol];
            var target = board[tRow][tCol];
            Console.WriteLine("attacker: " + attacker); 
            Console.WriteLine("target: " + target); 

            if (attacker == null || target == null)
            {
                Console.WriteLine("someone null");
                return false;
            }
            if (attacker.AvailableCaptures.Contains(target.Position) || attacker.AvailableMoves.Contains(target.Position))
            {
                return true;
            }
            Console.WriteLine("does not contain in list - " + attacker.AvailableCaptures.Count + " " + attacker.AvailableMoves.Count);
            Console.WriteLine("in lists: ");
            attacker.AvailableCaptures.ForEach(x => Console.WriteLine(x));
            attacker.AvailableMoves.ForEach(x => Console.WriteLine(x));
            return false; 
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

    }
}
