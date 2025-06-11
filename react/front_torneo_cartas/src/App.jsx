import Login from './components/Login'
import Autoregistro from './components/Autoregistro'
import Sesion from './components/Sesion';
import { Routes, Route } from 'react-router'
import ValidarAuth from './components/ValidarAuth';
import { useEffect, useState } from 'react';
import getSesionUserID  from './utils/getSesionUserID';


// import './styles/App.css'



function App() {

    const [isAuth, setIsAuth] = useState(false);

    useEffect(() => {
        const sesionUserID = getSesionUserID();
        
        if (sesionUserID) {
            setIsAuth(true)

            console.log("isAuth:",isAuth)
            console.log("sesion user id:",sesionUserID)            
        }

    }, [isAuth]);

  return (
    <Routes>
        <Route path="/" 
            element={
                <ValidarAuth >
                    <Sesion/>
                </ValidarAuth>} />

        <Route path="/login" element={<Login />} />
        <Route path="/registro" element={<Autoregistro />} />
    </Routes>
  )
}

export default App
