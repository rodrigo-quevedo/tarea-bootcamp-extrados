import Login from './components/Login'
import Autoregistro from './components/Autoregistro'
import { Routes, Route } from 'react-router'
// import './styles/App.css'

function App() {


  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/registro" element={<Autoregistro />} />
    </Routes>
  )
}

export default App
