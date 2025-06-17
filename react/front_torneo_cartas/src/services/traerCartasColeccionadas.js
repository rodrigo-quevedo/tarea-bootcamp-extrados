
import axios from "axios"
import qs from "qs"

export default async function traerCartasColeccionadas(setCartasColeccionadas) {

    try {
        const responseIDs = await axios.get(import.meta.env.VITE_URL_BACK+'/coleccion');

        // console.log("responseIDs:", responseIDs)
        // console.log("id_cartas_coleccionadas:", responseIDs.data.id_cartas_coleccionadas)
        // console.log("instanceof Array:",  responseIDs.data.id_cartas_coleccionadas  instanceof Array)
        
        if (responseIDs.data.id_cartas_coleccionadas  === null && responseIDs.data.id_cartas_coleccionadas  === undefined){
                console.error(responseIDs.data.message)
                setCartasColeccionadas(null)
        }
        
        
        const responseData = await axios.get(import.meta.env.VITE_URL_BACK+'/cartas', {
            // axios => ?id_cartas=1&id_cartas=2...
            params: {id_cartas: responseIDs.data.id_cartas_coleccionadas},
            paramsSerializer: params => qs.stringify(params, { arrayFormat: 'repeat' }) }
        );
        
        // console.log("responseIDs:", responseData)
        // console.log("cartas:", responseData.data.cartas)
        // console.log("instanceof Array:",  responseData.data.cartas  instanceof Array)
        
        if (responseData.status === 200){
            setCartasColeccionadas(responseData.data.cartas)
        }

    }
    catch(err){
        console.error("error axios buscar usuarios:",err)
    }
    
}