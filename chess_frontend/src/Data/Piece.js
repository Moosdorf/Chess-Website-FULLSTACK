class Piece {
    static tempId = 0;
    constructor(id, piece, color, row, col, moves, availableMoves) {
        this.id = id; 
        this.piece = piece;
        this.color = color;
        this.row = row;
        this.col = col;
        this.moves = moves;
        this.availableMoves = availableMoves;
    }
}

export default Piece;