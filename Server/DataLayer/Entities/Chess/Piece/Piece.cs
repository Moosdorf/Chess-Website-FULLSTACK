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
    public bool IsAlive { get; set; } = true;
    public bool IsWhite { get; set; }
    public bool CanMove { get; set; } = false;
    public List<string> AvailableMoves { get; set; } = [];
    public List<string> Attackers { get; set; } = [];
    public List<string> Defenders { get; set; } = [];
    public abstract void FindMoves(Piece[][] board);
    public abstract bool Move();
    public abstract bool Capture();

}
