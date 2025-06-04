import React from 'react'; 
import 'bootstrap/dist/css/bootstrap.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import Frontpage from './Pages/Frontpage';
import Chess from './Pages/Chess';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

function App() {
  return (
    <Container>

      <Navbar className="navbar-dark bg-dark border">
        <Navbar.Brand href="/">Home</Navbar.Brand>
      </Navbar>


      <BrowserRouter>
        <Routes>
          <Route index element={<Frontpage />}/>
          <Route path='chess_game' element={<Chess />}/>
        </Routes>
      </BrowserRouter>
    </Container>
  );
}

export default App;
