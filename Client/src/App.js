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


function App() {


  return (
    
    <BrowserRouter>
      <AuthProvider>
        <SignalRProvider>
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
        </SignalRProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
