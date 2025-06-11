
export default function getSesionRol() {
    const sesion = localStorage.getItem('sesion');
    if (!sesion) {
        console.error("La sesión expiró.")
        return null
    }


    try {
        return JSON.parse(sesion).rol;

    } catch (err){
        console.error(err)

        return null;
    }
    
}
