import { Link, useNavigate } from 'react-router-dom';
import { useForm, SubmitHandler, SubmitErrorHandler } from "react-hook-form";
import { Navbar, Nav, Container, Button, Row, Col } from 'react-bootstrap';
import { ClearCookies, GetCookies } from './Functions/HelperMethods';
import { useState } from 'react';
import { useAuth } from './Data/AuthProvider';


function SignInUp() {
  return (<Nav className="ms-auto">
            <Nav.Link as={Link} to="/sign_up">
              Sign Up
            </Nav.Link>

            <Nav.Link as={Link} to="/sign_in">
              Sign In
            </Nav.Link>
            
            </Nav>)
}

function ChessNavbar() {
  const [cookies, setCookies] = useState(GetCookies());
  const { user, signout } = useAuth();
  let navigate = useNavigate();

  function SignedIn() {
    const { register, handleSubmit, setError, formState: { errors } } = useForm()
    const handleSignOut = () => {
      navigate("/sign_in");
      signout();
    }
    return (
              <Nav className="ms-auto align-items-center gap-3">
                <Navbar.Text className="me-3 text-white">
                  Signed in as <strong>{user}</strong>
                </Navbar.Text>
                <Nav.Link as={Link} to="/user">Profile</Nav.Link>
                <Button className='text-center' variant="secondary" onClick={handleSignOut}>
                    Sign Out
                </Button>
              </Nav>)
  }

  return (
    <Navbar bg="dark" variant="dark">
      <Container fluid>
        <Navbar.Brand as={Link} to="/">
          Home
        </Navbar.Brand>
        <Nav className="me-auto my-2 my-lg-0"
            style={{ maxHeight: '100px' }}
            navbarScroll>
          <Nav.Link as={Link} to="/chess_game">Chess Game</Nav.Link>
        </Nav>

        {user ? <SignedIn/> : <SignInUp/>}

      </Container>
    </Navbar>
    )
}

export default ChessNavbar;