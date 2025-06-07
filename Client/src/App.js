import './index.css';
import 'bootstrap/dist/css/bootstrap.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import Frontpage from './Pages/Frontpage';
import Chess from './Pages/Chess';
import Navbar from 'react-bootstrap/Navbar';
import SignUp from './Pages/SignUp';

function App() {
  return (
    <Container>

      <Navbar className="navbar-dark bg-dark border header-navbar">
        <Navbar.Brand href="/">Home</Navbar.Brand>
      </Navbar>


      <BrowserRouter>
        <Routes>
          <Route index element={<Frontpage />}/>
          <Route path='chess_game' element={<Chess />}/>
          <Route path='sign_up' element={<SignUp />}/>
        </Routes>
      </BrowserRouter>
    </Container>
  );
}

export default App;
