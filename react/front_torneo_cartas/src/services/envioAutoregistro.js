import axios from 'axios';


export default async function manejoLogin(datosUsuario, navigate) {

    //agregar rol = jugador
    datosUsuario.rol = "jugador"

    //sacar confirmPassword (no hace falta enviarlo al back)
    delete datosUsuario.confirmPassword;

    //hacer POST
    try {
        const response = await axios.post(import.meta.env.VITE_URL_BACK+'/registro', datosUsuario);
        console.log('Autoregistro ok:', response.data.message);

        navigate("/login")


    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);

    }


}