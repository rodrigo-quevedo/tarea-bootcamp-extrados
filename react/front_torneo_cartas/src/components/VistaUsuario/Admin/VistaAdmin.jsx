import Header from "./Header/Header"
import Footer from "../../Footer/Footer"

import { Outlet } from "react-router";

export default function VistaAdmin(){

    return (
        <>
            <Header />

            <main>
                <Outlet/>
            </main>
            
            <Footer />
        </>
    )

}