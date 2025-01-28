class Piece {
    static tempId = 0;
    constructor(piece, color, id) {
        this.id = id; 
        this.piece = piece;
        this.color = color;
    }
}

export default Piece;