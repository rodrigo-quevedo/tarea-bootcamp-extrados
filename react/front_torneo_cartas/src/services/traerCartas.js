// import { useEffect } from "react";
import axios from "axios"

export default async function traerCartas(setCartas) {

    try {

        const response = await axios.get(import.meta.env.VITE_URL_BACK+'/cartas/all');

        // console.log("response:", response)
        // console.log("cartas:", response.data.cartas)
        // console.log("instanceof Array:",  response.data.cartas.series instanceof Array)
        
        if (response.data.cartas !== null && response.data.cartas !== undefined)
            setCartas(response.data.cartas);

        else setCartas(null)

    }
    catch(err){
        console.error("error axios buscar usuarios:",err)
    }
    
}