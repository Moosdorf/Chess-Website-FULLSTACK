import { Link, useNavigate } from 'react-router-dom';
import { Navbar, Nav, Container, Button, NavDropdown } from 'react-bootstrap';
import { useAuth } from '../Data/AuthProvider';
import ChessTypeSelection from './ChessTypeSelection';
import { GetCookies } from '../Functions/HelperMethods';
import { useState } from 'react';

function ChessNavbar() {
  var cookies = GetCookies();
  const { user, signout } = useAuth();
  const navigate = useNavigate();
  const [showBotOptions, setShowBotOptions] = useState(false);

  const handleShow = () => setShowBotOptions(true);
  const handleClose = () => {
      setShowBotOptions(false);
  }
  const handleSelection = (playerWhite) => {
      if (cookies.user) navigate("/chess_game", { state: { botGame: true, playerWhite: playerWhite} })
        handleClose(true);
  }

  const handleSignOut = () => {
    signout();
    navigate("/sign_in");
  };

  return (
     <Navbar bg="dark" variant="dark" expand="lg" className="chess-navbar">
      <Container fluid>
        <Navbar.Brand as={Link} to="/" className="navbar-brand">
          <i className="fas fa-chess-knight brand-icon"></i>
          home
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="navbar-content" className="navbar-toggle" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <NavDropdown 
              title="Play" 
              id="play-dropdown" 
              menuVariant="dark"
              className="nav-dropdown"
            >
              <NavDropdown.Item as={Link} onClick={handleShow}>
                <i className="fas fa-robot me-2"></i> vs Computer                
              </NavDropdown.Item>
              <ChessTypeSelection show={showBotOptions} handleClose={handleClose} handleSelection={handleSelection} />

              <NavDropdown.Divider className="dropdown-divider" />
              <NavDropdown.Item as={Link} to="/chess_game" className="dropdown-item">
                <i className="fas fa-users me-2"></i> Multiplayer
              </NavDropdown.Item>
            </NavDropdown>
            <Nav.Link as={Link} to="/puzzles" className="nav-link">
              <i className="fas fa-puzzle-piece me-1"></i> Puzzles
            </Nav.Link>
            <Nav.Link as={Link} to="/learn" className="nav-link">
              <i className="fas fa-graduation-cap me-1"></i> Learn
            </Nav.Link>
          </Nav>
          
          <Nav className="align-items-center">
            {user ? (
              <>
                <div className="user-greeting me-3">
                  <i className="fas fa-user-circle me-2"></i>
                  <span>{user}</span>
                </div>
                <Button 
                  as={Link} 
                  to="/user" 
                  className="profile-btn me-2"
                >
                  Profile
                </Button>
                <Button 
                  onClick={handleSignOut}
                  className="signout-btn"
                >
                  <i className="fas fa-sign-out-alt me-1"></i> Sign Out
                </Button>
              </>
            ) : (
              <>
                <Button 
                  as={Link} 
                  to="/sign_up" 
                  className="signup-btn me-2"
                >
                  Sign Up
                </Button>
                <Button 
                  as={Link} 
                  to="/sign_in" 
                  className="signin-btn"
                >
                  Sign In
                </Button>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}

export default ChessNavbar;