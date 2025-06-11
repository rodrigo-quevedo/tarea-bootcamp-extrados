
import Footer from "./Footer/Footer"
import Header from "./Header/Header"
import { useNavigate } from "react-router"
import getSesionRol from "../utils/getSesionRol";

export default function Sesion(){

    // Dependiendo el rol es lo que se retorna

    switch(getSesionRol()) {
        
    }

    return (
        <>

            <Header />            

            <h1>Sesion</h1>

            <Footer />
        </>


        
    )

}