import axios from 'axios';
import { rutas } from '../config/rutas';


export default async function crearUsuario(datosUsuario, navigate) {

    //sacar confirmPassword (no hace falta enviarlo al back)
    delete datosUsuario.confirmPassword;

    //hacer POST
    try {
        const response = await axios.post(import.meta.env.VITE_URL_BACK+'/crear', datosUsuario);
        console.log('Usuario creado OK:', response.data.message);

        navigate(rutas.admin.usuarios)

    } catch (error) {
      console.error('Crear error:', error.response?.data || error.message);

    }


}