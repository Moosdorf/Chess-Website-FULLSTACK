import { useState } from 'react';

export function useChessPieceLogic(piece, selectedPiece, setSelectedPiece, chessState, movePiece, ourTurn) {
  const [promotionInfo, setPromotionInfo] = useState(null);
  const promotionTypes = ["queen", "rook", "bishop", "knight"];

  const handleClick = () => {
    if (!ourTurn || chessState.gameDone) return;

    if (!selectedPiece) {
      setSelectedPiece(piece);
      return;
    }

    if (selectedPiece !== piece) {
      const canMove = selectedPiece.AvailableMoves?.includes(piece.Position) ||
                      selectedPiece.AvailableCaptures?.includes(piece.Position);
      if (canMove) {
        const targetRank = piece.Position[1];
        const isPromotionRank = (selectedPiece.IsWhite && targetRank === '8') || (!selectedPiece.IsWhite && targetRank === '1');

        if (selectedPiece.Type === "pawn" && isPromotionRank) {
          const promotionSquares = promotionTypes.map(type => ({
            position: piece.Position,
            type,
            color: selectedPiece.IsWhite ? "white" : "black",
            onClick: (selectedType) => {
              movePiece(selectedPiece, piece, selectedType);
              setPromotionInfo(null);
              setSelectedPiece(null);
            }
          }));
          setPromotionInfo({ from: selectedPiece, to: piece, squares: promotionSquares });
          return;
        }

        movePiece(selectedPiece, piece, null);
      }
      setSelectedPiece(piece);
      return;
    }

    setSelectedPiece(null);
  };

  const onDragStart = (e) => {
    if (!ourTurn || piece.IsWhite !== chessState.isWhitesTurn) return;
    e.dataTransfer.setData("text", JSON.stringify(piece));
  };

  const onDragEnd = () => {};

  return { handleClick, onDragStart, onDragEnd, promotionInfo, setPromotionInfo };
}
