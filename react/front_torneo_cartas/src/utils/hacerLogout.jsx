import axios from "axios";

export default function hacerLogout(navigate){
        localStorage.removeItem('sesion');
        localStorage.removeItem('jwt');

        axios.defaults.headers.common['Authorization'] = null;

        navigate('/login');
}