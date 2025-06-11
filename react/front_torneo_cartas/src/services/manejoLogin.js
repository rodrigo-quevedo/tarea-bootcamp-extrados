import axios from 'axios';
import { jwtDecode } from "jwt-decode";



export default async function manejoLogin(e, datosUsuario, navigate) {
    e.preventDefault(); 
    console.log(`form submit. Datos: ${JSON.stringify(datosUsuario)}`)




    try {
      const response = await axios.post(import.meta.env.VITE_URL_BACK+'/login', datosUsuario);
      console.log('Login ok:', response.data.message);

      // manejo login: 1. parsear el JWT, 2. guardar datos JWT en storage, 3. armar Bearer
      
        if (!response.data.jwt) {
            console.error("No se encontró JWT en la server response.");
            return;
        }

        const payload = jwtDecode(response.data.jwt);
        console.log("el payload:", payload)

        localStorage.setItem('sesion', JSON.stringify({
            id_usuario: payload[import.meta.env.VITE_CLAIMS_ID_USUARIO],
            email: payload[import.meta.env.VITE_CLAIMS_EMAIL],
            rol: payload[import.meta.env.VITE_CLAIMS_ROL]
        }));

        axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.jwt}`;

        navigate('/');

    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);

    }

}