/* import logo from './logo.svg'; */
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import './App.css';
import Login from './Login';
import Register from './Register';
import Dashboard from './Dashboard';
import ForgotPassword from './forgot-password';

function App() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<Login/>}></Route>
        <Route path='/register' element={<Register/>}></Route>
        <Route path='/forgot-password' element={<ForgotPassword/>}></Route>
        <Route path='/Dashboard' element={<Dashboard/>}></Route>
      </Routes>
    </Router>
  );
}

export default App;
