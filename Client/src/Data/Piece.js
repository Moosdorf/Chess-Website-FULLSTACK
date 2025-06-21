class Piece {
    constructor(Type, IsWhite, Position, Moves, Captures, AvailableMoves, AvailableCaptures, Defenders, Attackers, IsAlive) {
        this.Type = Type; 
        this.IsWhite = IsWhite;
        this.Position = Position;
        this.Moves = Moves;
        this.Captures = Captures;
        this.AvailableMoves = AvailableMoves;
        this.AvailableCaptures = AvailableCaptures;
        this.Defenders = Defenders;
        this.Attackers = Attackers;
        this.IsAlive = IsAlive;
    }
}

export default Piece;