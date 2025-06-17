import axios from 'axios';
import { rutas } from '../config/rutas';


//viene validado desde el handler
export default async function coleccionarCartas(arrayIDCartasSeleccionadas, setPopupError, setPopupExito) {

    const requestBody = {
        Id_cartas: arrayIDCartasSeleccionadas
    }

    //hacer POST
    try {
        const response = await axios.post(import.meta.env.VITE_URL_BACK+'/coleccion', requestBody);
        console.log('Cartas agregadas OK:', response.data.message);

        if (response.status === 200)
            setPopupExito(`
                \nMensaje: ${response.data.message}
                \n\nRepetidas:${JSON.stringify(response.data.id_cartas_repetidas)}
                \n\Agregadas:${JSON.stringify(response.data.id_cartas_agregadas)}
                `)

        else setPopupError(response.data.message)

    } catch (error) {
        console.error('Crear error:', error.response?.data || error.message);   
        setPopupError(error.message)

    }


}