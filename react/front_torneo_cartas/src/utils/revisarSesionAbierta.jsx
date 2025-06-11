
import { useEffect } from "react";

export default function revisarSesionAbierta(navigate){

    useEffect(()=>{
        const sesion = localStorage.getItem('sesion');
        if (sesion != null) navigate("/")

        return;
    }, [])
}