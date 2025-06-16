// import { useEffect } from "react";
import axios from "axios"

export default async function traerUsuarios(setUsuarios) {

    try {
        // console.log('axios default en buscar usuarios:',axios.defaults.headers.common);

        const response = await axios.get(import.meta.env.VITE_URL_BACK+'/usuarios/activos');

        // console.log("response:", response)
        // console.log("usuarios:", response.data.usuarios)
        // console.log("instanceof Array:",  response.data.usuarios instanceof Array)
        
        setUsuarios(response.data.usuarios);
    }
    catch(err){
        console.error("error axios buscar usuarios:",err)
    }
    
}