import Login from './components/Login'
import { Routes, Route } from 'react-router'
// import './styles/App.css'

function App() {


  return (
    <Routes>
      <Route path="/login" element={<Login />} />
    </Routes>
  )
}

export default App
