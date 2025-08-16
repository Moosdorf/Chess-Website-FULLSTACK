import './index.css';
import 'bootstrap/dist/css/bootstrap.css';
import './ChessStyle.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Container } from 'react-bootstrap';
import Frontpage from './Pages/Frontpage';
import Chess from './Pages/Chess';
import SignUp from './Pages/SignUp';
import SignIn from './Pages/SignIn';
import Userpage from './Pages/User';
import { AuthProvider } from './Data/AuthProvider';
import ChessNavbar from './Components/ChessNavbar';
import { SignalRProvider } from './SignalR/SignalRProvider';
import { SignalRGameProvider } from './SignalR/SingalRGameProvider';
import Puzzle from './Pages/Puzzle';


function App() {


  return (
    
    <BrowserRouter>
      <AuthProvider>
        <SignalRProvider>
          <SignalRGameProvider>
            <Container>
              <ChessNavbar/>
              <Routes>
                <Route index element={<Frontpage />}/>
                <Route path='chess_game/:id' element={<Chess />}/>
                <Route path='sign_up' element={<SignUp />}/>
                <Route path='sign_in' element={<SignIn />}/>
                <Route path='user' element={<Userpage />}/>
                <Route path='puzzles' element={<Puzzle />}/>
              </Routes>
            </Container>
          </SignalRGameProvider>
        </SignalRProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
