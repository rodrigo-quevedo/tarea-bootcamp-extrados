
import { useNavigate } from "react-router"
import getSesionRol from "../utils/getSesionRol";
import { useEffect } from "react";

// import Footer from "./Footer/Footer";

export default function Sesion(){

    const navigate = useNavigate();

    // Dependiendo el rol es lo que se retorna
    const rol = getSesionRol()

    console.log("rol", rol)
        
    useEffect(()=>{
        switch(rol) {

            case 'admin': {
                console.log("admin rol")
                navigate("/admin")
                return;
            }
            case 'organizador': {
                navigate("/organizador")
                return;
            }
            case 'juez': {
                navigate("/juez")
                return;
            }
            case 'jugador': {
                navigate("/jugador")
                return;
            }

            default : return <h1>404 not found</h1>
        }
    }, [])
}