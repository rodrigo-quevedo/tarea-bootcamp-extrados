import { Navigate } from 'react-router';


export default function ValidarAuth({ children }){

    const sesion = localStorage.getItem('sesion')

    if (sesion == null) return <Navigate to="/login" replace />;
    
    return children;
}