import axios from 'axios';
import { rutas } from '../config/rutas';


export default async function editarUsuario(datosUsuario, ) {

    //sacar confirmPassword (no hace falta enviarlo al back)
    delete datosUsuario.confirmPassword;

    //hacer POST
    try {
        const response = await axios.put(import.meta.env.VITE_URL_BACK+`/usuarios/${datosUsuario?.id}`, datosUsuario);
        console.log('Usuario editado OK:', response.data.message);

        

    } catch (error) {
      console.error('Crear error:', error.response?.data || error.message);

    }


}