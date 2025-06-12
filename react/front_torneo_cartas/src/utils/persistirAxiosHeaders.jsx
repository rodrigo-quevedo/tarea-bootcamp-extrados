import { useEffect } from "react";

import axios from "axios";

export default function persistirAxiosHeaders(){

    useEffect(()=>{
        // console.log("persistiendo headers")

        //persistir headers de axios cuando se recarga la pagina        
        const token = localStorage.getItem('jwt');
        if (token) {
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        }   
    }, [])
}