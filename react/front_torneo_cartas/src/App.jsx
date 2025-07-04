import Login from './components/Login'
import Autoregistro from './components/Autoregistro'
import Sesion from './components/Sesion';
import { Routes, Route } from 'react-router'
import ValidarAuth from './components/ValidarAuth';
import { useEffect, useState } from 'react';
import getSesionUserID  from './utils/getSesionUserID';
import VistaAdmin from './components/VistaUsuario/Admin/VistaAdmin';
import VistaJugador from './components/VistaUsuario/Jugador/VistaJugador';
import Usuarios from './components/VistaUsuario/Admin/Pantallas/Usuarios';
import Torneos from './components/VistaUsuario/Admin/Pantallas/Torneos'
import Partidas from './components/VistaUsuario/Admin/Pantallas/Partidas'
import CrearUsuario from './components/VistaUsuario/Admin/Pantallas/CrearUsuario';
import Coleccion from "./components/VistaUsuario/Jugador/Pantallas/Coleccion"
import Mazo from "./components/VistaUsuario/Jugador/Pantallas/Mazo"

import persistirAxiosHeaders from "./utils/persistirAxiosHeaders"
import useScrollToTop from './hooks/useScrollToTop';
import { validarProtocoloYDominio } from './utils/validations';

// import './styles/App.css'



function App() {

    useEffect(() => {
        const sesionUserID = getSesionUserID();

        if (sesionUserID) console.log("sesion user id:",sesionUserID)   
    }, []);

    persistirAxiosHeaders();

    useScrollToTop();



  return (
    <Routes>
        <Route path="/" element={<ValidarAuth><Sesion/></ValidarAuth>} />

        <Route path="login" element={<Login />} />
        <Route path="registro" element={<Autoregistro />} />

        {/* <Route path="/admin" element={<VistaAdmin/>} /> */}
        
        <Route path="admin"  element={<ValidarAuth><VistaAdmin/></ValidarAuth>} >
            <Route index element={<h1 style={{padding:"0 0 77vh 0"}}>Panel admin</h1>}/>
            
            <Route path="usuarios" element={<Usuarios/>}/>
            <Route path="usuarios/crear" element={<CrearUsuario/>}/>
            <Route path="torneos" element={<Torneos/>} />
            <Route path="partidas" element={<Partidas/>} />
        </Route>

        <Route path="jugador"  element={<ValidarAuth><VistaJugador/></ValidarAuth>} >
            <Route index element={<h1 style={{padding:"0 0 77vh 0"}}>Panel jugador</h1>}/>
            
            <Route path="coleccion" element={<Coleccion/>}/>
            <Route path="mazo" element={<Mazo/>}/>
        </Route>



    </Routes>
  )
}

export default App
