class Piece {
    constructor(Type, IsWhite, Position, Moves, AvailableMoves, Defenders, Attackers, IsAlive) {
        this.Type = Type; 
        this.IsWhite = IsWhite;
        this.Position = Position;
        this.Moves = Moves;
        this.AvailableMoves = AvailableMoves;
        this.Defenders = Defenders;
        this.Attackers = Attackers;
        this.IsAlive = IsAlive;
    }
}

export default Piece;