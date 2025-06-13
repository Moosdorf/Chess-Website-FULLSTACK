import './index.css';
import 'bootstrap/dist/css/bootstrap.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import Frontpage from './Pages/Frontpage';
import Chess from './Pages/Chess';
import ChessNavbar from './ChessNavbar';
import SignUp from './Pages/SignUp';
import SignIn from './Pages/SignIn';
import Userpage from './Pages/User';
import { AuthProvider } from './Data/AuthProvider';

function App() {
  return (
    
    <BrowserRouter>
      <AuthProvider>
        <Container>
          <ChessNavbar/>
          <Routes>
            <Route index element={<Frontpage />}/>
            <Route path='chess_game' element={<Chess />}/>
            <Route path='sign_up' element={<SignUp />}/>
            <Route path='sign_in' element={<SignIn />}/>
            <Route path='user' element={<Userpage />}/>
          </Routes>
        </Container>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
