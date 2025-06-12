import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router';
import axios from 'axios';

import AutoStoriesIcon from '@mui/icons-material/AutoStories';

import "./Header.css"

import {rutas} from "../../../../config/rutas"


function focusBoton (rutaString){
    return (window.location.pathname === rutaString) ? "admin-header-nav-activo" : null
}



export default function Header(){

    const navigate = useNavigate();

    const hacerLogout = () => {
        localStorage.removeItem('sesion');

        axios.defaults.headers.common['Authorization'] = null;

        navigate('/login');
    };

    return (
        <AppBar position="static" color="primary" sx={{ px: 2 }}>
            <Toolbar sx={{ justifyContent: 'space-between', display:'flex', flexWrap:'wrap' }}>
                {/* Logo */}
                <Typography
                    variant="h6"
                    component={Link}
                    to="/"
                    sx={{ textDecoration: 'none', fontWeight: 'bold', alignItems:'center', display: 'flex', gap: '8px' }}
                >
                <AutoStoriesIcon/> Torneo de Cartas
                </Typography>

                {/* Navigation */}
                <Box sx={{ display: 'flex', gap: 2, flexWrap: "wrap" }}>
                
                    <Button component={Link} to={rutas.admin.usuarios} color="inherit" className={focusBoton(rutas.admin.usuarios)}>
                    Usuarios
                    </Button>
                    
                    <Button component={Link} to={rutas.admin.torneos} color="inherit" className={focusBoton(rutas.admin.torneos)}>
                    Torneos
                    </Button>
                    
                    <Button component={Link} to={rutas.admin.partidas} color="inherit" className={focusBoton(rutas.admin.partidas)}>
                    Partidas
                    </Button>

                    <Button onClick={hacerLogout} color="info" sx={{backgroundColor:"#000"}}>
                    Cerrar sesión
                    </Button>

                </Box>

            </Toolbar>
        </AppBar>
    )

}
