using DataLayer.Entities.Chess;
using DataLayer.HelperMethods;
using DataLayer.Models.Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class ChessInfo
{
    public Piece[][] GameBoard { get; private set; }
    public List<Piece> BlackPieces { get; private set; } = [];
    public List<Piece> WhitePieces { get; private set; } = [];
    public King BlackKing { get; private set; } = null!;
    public King WhiteKing { get; private set; } = null!;
    public bool InCheck { get; set; } = false;
    public King? CheckedKing { get; set; } = null;
    public List<string> Blockers { get; set; } = [];
    public int Moves { get; set; }
    

    public ChessInfo()
    {
        this.GameBoard = CreateGameBoard();
        InitializeInfo();
        FindAvailableMoves();
    }
    public ChessInfo(List<Move> moves)
    {
        this.GameBoard = CreateGameBoard();
        InitializeInfo();
        if (moves.Count == 0) FindAvailableMoves();
        else ReplayMoves(moves);
    }

    private void InitializeInfo()
    {
        foreach (Piece[] pieces in GameBoard)
        {
            foreach (Piece piece in pieces)
            {
                if (piece.Type == PieceType.Empty) continue;

                if (piece.IsWhite)
                {
                    WhitePieces.Add(piece);
                    if (piece.Type == PieceType.King)
                    {
                        WhiteKing = (King)piece;
                    }
                }
                else
                {
                    BlackPieces.Add(piece);
                    if (piece.Type == PieceType.King)
                    {
                        BlackKing = (King) piece;
                    }
                }
            }
        }
    }


    private void ReplayMoves(List<Move> moves)
    {
        // replay game to get to current state
        for (int i = 0; i < moves.Count; i++)
        {
            ChessMethods.MakeMove(GameBoard, moves[i].MoveString);
            Moves++;
            FindAvailableMoves();
        }

    }

    public bool Move(string move)
    {
        // check if move is good
        var canMove = ChessMethods.ValidateMove(move, GameBoard);
        if (!canMove) return false;

        // make the move
        Console.WriteLine("Move: " + Moves);
        ChessMethods.MakeMove(GameBoard, move);
        Moves++;
        FindAvailableMoves();
        return true;
    }


    public void FindAvailableMoves()
    {
        CheckedKing = null;
        InCheck = false;
        Blockers = [];
        // reset all piece stats
        foreach (var piece in GameBoard.SelectMany(row => row))
        {
            piece.AvailableMoves = new();
            piece.AvailableCaptures = new();
            piece.Attackers = new();
            piece.Defenders = new();
        }

        Console.WriteLine("moves: " + Moves);
        if (Moves % 2 == 0)
        {
            Console.WriteLine("finding black moves");   
            foreach (var piece in BlackPieces)
            {
                piece.FindMoves(this);
            }
            Console.WriteLine("finding white moves");
            foreach (var piece in WhitePieces)
            {
                piece.FindMoves(this);
            }
        } else
        {
            Console.WriteLine("finding white moves");
            foreach (var piece in WhitePieces)
            {
                piece.FindMoves(this);
            }
            Console.WriteLine("finding black moves");
            foreach (var piece in BlackPieces)
            {
                piece.FindMoves(this);
            }
        }
    }

    public Piece[][] CreateGameBoard()
    {
        var chessBoard = new Piece[8][]; // ends as final result 

        for (int row = 0; row < 8; row++) // initiate each row in the jagged array (must be done otherwise we have no arrays to push to)
        {
            chessBoard[row] = new Piece[8];
        }

        chessBoard[0][0] = new Rook(true) { Type = PieceType.Rook, Position = ChessMethods.RowColToRankFile(0, 0) };
        chessBoard[0][1] = new Knight(true) { Type = PieceType.Knight, Position = ChessMethods.RowColToRankFile(0, 1) };
        chessBoard[0][2] = new Bishop(true) { Type = PieceType.Bishop, Position = ChessMethods.RowColToRankFile(0, 2) };
        chessBoard[0][3] = new Queen(true) { Type = PieceType.Queen, Position = ChessMethods.RowColToRankFile(0, 3) };
        chessBoard[0][4] = new King(true) { Type = PieceType.King, Position = ChessMethods.RowColToRankFile(0, 4) };
        chessBoard[0][5] = new Bishop(true) { Type = PieceType.Bishop, Position = ChessMethods.RowColToRankFile(0, 5) };
        chessBoard[0][6] = new Knight(true) { Type = PieceType.Knight, Position = ChessMethods.RowColToRankFile(0, 6) };
        chessBoard[0][7] = new Rook(true) { Type = PieceType.Rook, Position = ChessMethods.RowColToRankFile(0, 7) };

        chessBoard[7][0] = new Rook(false) { Type = PieceType.Rook, Position = ChessMethods.RowColToRankFile(7, 0) };
        chessBoard[7][1] = new Knight(false) { Type = PieceType.Knight, Position = ChessMethods.RowColToRankFile(7, 1) };
        chessBoard[7][2] = new Bishop(false) { Type = PieceType.Bishop, Position = ChessMethods.RowColToRankFile(7, 2) };
        chessBoard[7][3] = new Queen(false) { Type = PieceType.Queen, Position = ChessMethods.RowColToRankFile(7, 3) };
        chessBoard[7][4] = new King(false) { Type = PieceType.King, Position = ChessMethods.RowColToRankFile(7, 4) };
        chessBoard[7][5] = new Bishop(false) { Type = PieceType.Bishop, Position = ChessMethods.RowColToRankFile(7, 5) };
        chessBoard[7][6] = new Knight(false) { Type = PieceType.Knight, Position = ChessMethods.RowColToRankFile(7, 6) };
        chessBoard[7][7] = new Rook(false) { Type = PieceType.Rook, Position = ChessMethods.RowColToRankFile(7, 7) };



        // creating and pushing black pawns
        for (int col = 0; col < 8; col++)
        {
            chessBoard[1][col] = new Pawn(true) { Type = PieceType.Pawn, Position = ChessMethods.RowColToRankFile(1, col) }; // insert white pawns
            chessBoard[6][col] = new Pawn(false) { Type = PieceType.Pawn, Position = ChessMethods.RowColToRankFile(6, col) }; // insert black pawns

        }

        for (int row = 2; row < 6; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                chessBoard[row][col] = new Empty(false) { Type = PieceType.Empty, Position = ChessMethods.RowColToRankFile(row, col) };
            }
        }

        return chessBoard;
    }

    public override string? ToString()
    {
        var blackKingPos = BlackKing != null ? BlackKing.Position.ToString() : "null";
        var whiteKingPos = WhiteKing != null ? WhiteKing.Position.ToString() : "null";

        return $"Black Pieces: {BlackPieces?.Count ?? 0}\n" +
               $"White Pieces: {WhitePieces?.Count ?? 0}\n" +
               $"Black King Position: {blackKingPos}\n" +
               $"White King Position: {whiteKingPos}\n" +
               $"Moves: {Moves}";
    }

}
