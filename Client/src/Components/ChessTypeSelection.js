import { Button, Modal } from "react-bootstrap";
import { GetCookies } from "../Functions/HelperMethods";
import { useNavigate } from "react-router-dom";

function ChessTypeSelection({ show, handleClose, handleSelection }) {
    const cookies = GetCookies();
    const notSignedIn = cookies.user == null;
    let navigate = useNavigate();

    const handleLogin = () => {
        handleSelection(false);
        navigate("/sign_in");
    }

    return (
        <Modal 
            show={show} 
            onHide={handleClose} 
            centered
            className="chess-selection-modal"
        >
            <Modal.Header closeButton className="modal-header">
                <Modal.Title className="modal-title">
                    <i className="fas fa-chess-board me-2"></i>
                    Choose Your Side
                </Modal.Title>
            </Modal.Header>
            
            {notSignedIn && (
                <Modal.Body className="signin-warning">
                    <div className="warning-content">
                        <i className="fas fa-exclamation-triangle warning-icon"></i>
                        <span>Please sign in to play</span>
                        <Button 
                        onClick={() => handleLogin()}
                        variant="outline-warning">
                            Sign In
                        </Button>
                    </div>
                </Modal.Body>
            )}

            {!notSignedIn && (<Modal.Footer className="modal-footer">
                <Button
                    variant="light" 
                    onClick={() => handleSelection(true)}
                    className="white-side-btn"
                >
                    <i className="fas fa-chess-king me-2"></i>
                    Play as White
                </Button>
                <Button 
                    
                    variant="dark" 
                    onClick={() => handleSelection(false)}
                    className="black-side-btn"
                >
                    <i className="fas fa-chess-king me-2"></i>
                    Play as Black
                </Button>
            </Modal.Footer>)}
        </Modal>
    );
}

export default ChessTypeSelection;