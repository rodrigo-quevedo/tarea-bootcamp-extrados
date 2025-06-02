import axios from 'axios';
import { jwtDecode } from "jwt-decode";


export default async function manejoLogin(e, datosUsuario) {
    e.preventDefault(); 
    console.log(`form submit. Datos: ${JSON.stringify(datosUsuario)}`)



    try {
      const response = await axios.post(import.meta.env.VITE_URL_BACK+'/login', datosUsuario);
      console.log('Login ok:', response.data);

      // manejo login: 1. parsear el JWT, 2. guardar datos JWT en storage, 3. armar Bearer
      //   const decoded = jwtDecode(token);
      


    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);

      // optionally show error to the user
    }

}