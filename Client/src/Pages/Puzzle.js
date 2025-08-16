import { useCallback, useEffect, useState } from "react";
import { Card, CardBody, Col, Container, Row } from "react-bootstrap";
import Piece from "../Data/Piece";
import ChessBoardPiece from "../Components/ChessBoardPiece";

function Puzzle() {
    const [puzzle, setPuzzle] = useState(null);

    const handleSetPuzzle = useCallback((apiBoard) => {
        let tempChessBoard = apiBoard.chessboard.gameBoard.map(row =>
            row.map(piece => new Piece (
                piece.type, 
                piece.isWhite, 
                piece.position, 
                piece.pinned,
                piece.moves,
                piece.captures,
                piece.availableMoves,
                piece.availableCaptures,
                piece.defenders, 
                piece.attackers,
                piece.isAlive))
        );
        const fenSplit = apiBoard.fen.split(" ");

        setPuzzle({
            board: tempChessBoard,
            puzzleId: apiBoard.puzzleId,
            fen: apiBoard.fen,
            moves: parseInt(fenSplit[5]) * 2 + (fenSplit[1] === "b"),
            puzzleMoves: apiBoard.moves,
            tags: apiBoard.tags,            
            openingTags: apiBoard.openingTags,

        });
    }, []);

    useEffect(() => {
        
        async function GetPuzzle() {
            const request = new Request(`http://localhost:5000/api/puzzle/random`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json", 
                }
            });

            fetch(request)
            .then(res => res.text())
            .then(data => JSON.parse(data))
            .then(results => {
                console.log(results);
                handleSetPuzzle(results);
            })
            .catch(e => console.log(e));
        } 
        GetPuzzle();

    }, [])
    useEffect(() => {
        console.log(puzzle);
    }, [puzzle])
    return (
    <Container>

        <Row>
            {/* info pane 1 */}
            <Col md={2}>
                <Card>
                    <CardBody>
                        puzzles
                    </CardBody>
                </Card>

            </Col>

            {/* game */}
            <Col>
                <Card>
                    <CardBody>
                        {
                            puzzle.board.map((row, rowIndex) =>
                              row.map((piece, colIndex) => {
                                  const rowCol = [rowIndex, colIndex];
                                  return (
                                    <div className="wrapper">
                                        <div className="chessboard">
                                            <ChessBoardPiece
                                                key={`${rowIndex}-${colIndex}`}
                                                piece={piece}
                                                rowCol={rowCol}
                                            />
                                        </div>
                                    </div>
                                  );
                              })
                          )
                        }
                    </CardBody>
                </Card>
            </Col>

            {/* info pane 2 */}
            <Col md={2}>
                <Card>
                    <CardBody>
                        puzzles
                    </CardBody>
                </Card>

            </Col>
        </Row>

    </Container>
);
}
  
export default Puzzle;