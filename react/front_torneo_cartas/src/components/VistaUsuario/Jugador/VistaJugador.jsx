import HeaderJugador from "./Header/HeaderJugador"
import Footer from "../../Footer/Footer"

import { Outlet } from "react-router";

export default function VistaJugador(){

    return (
        <>
            <HeaderJugador />

            <main>
                <Outlet/>
            </main>
            
            <Footer />
        </>
    )

}