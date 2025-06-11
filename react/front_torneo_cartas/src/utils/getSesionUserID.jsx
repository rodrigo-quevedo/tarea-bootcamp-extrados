
export default function getSesionUserID() {
    const sesion = localStorage.getItem('sesion');
    if (!sesion) {
        console.error("La sesión expiró.")
        return null
    }


    try {
        return JSON.parse(sesion).id_usuario;

    } catch (err){
        console.error(err)

        return null;
    }
    
}
