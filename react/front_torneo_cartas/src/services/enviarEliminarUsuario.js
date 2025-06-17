import axios from 'axios';
import { rutas } from '../config/rutas';


export default async function enviarEliminarUsuario(id, setEliminarUsuario) {

    if (id == 1 || id == 105 || id == 89 || id == 101) return; //no quiero borrar las cuentas de prueba

    try {
        const response = await axios.delete(import.meta.env.VITE_URL_BACK+`/usuarios/${id}`);
        console.log('Eliminar usuario OK:', response.data.message);

        console.log(response)

        if (response.status === 200){
            setEliminarUsuario(null)
        }


        

    } catch (error) {
      console.error('Eliminar error:', error.response?.data || error.message);

    }


}