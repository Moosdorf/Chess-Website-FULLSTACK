using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Chess.Piece;

public abstract class Piece
{
    public Piece(bool white) 
    {
        IsWhite = white;
    }
    public string Type { get; set; }
    public string Position { get; set; } = string.Empty;
    public int Moves { get; set; } = 0;
    public int Captures { get; set; } = 0;
    public bool IsAlive { get; set; } = true;
    public bool IsWhite { get; set; }
    public bool CanMove { get; set; } = false;
    public List<string> AvailableMoves { get; set; } = [];
    public List<string> AvailableCaptures { get; set; } = [];
    public List<string> Attackers { get; set; } = [];
    public List<string> Defenders { get; set; } = [];
    public abstract void FindMoves(Piece[][] board);
    public abstract bool Move();
    public abstract bool Capture();
    public void AddMove(Piece piece)
    {
        AvailableMoves.Add(piece.Position);
        if (piece.IsWhite == this.IsWhite) piece.Defenders.Add(this.Position);
        else  piece.Attackers.Add(this.Position);
    }

    public void AddCaptures(Piece piece)
    {
        AvailableCaptures.Add(piece.Position);
        piece.Attackers.Add(this.Position);
    }


    public bool CheckSquare(Piece[][] board, int iRow, int iCol)
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
}
