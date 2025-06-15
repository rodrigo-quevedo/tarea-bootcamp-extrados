import { AppBar, Toolbar, Typography, Button, Box} from '@mui/material';
import { Link, useNavigate } from 'react-router';
// import axios from 'axios';

// import AutoStoriesIcon from '@mui/icons-material/AutoStories';

import ContrastIcon from '@mui/icons-material/Contrast';

import "./Header.css"

import {rutas} from "../../../../config/rutas"
import hacerLogout from '../../../../utils/hacerLogout';
import {useTema} from '../../../../hooks/useCambiarTheme.jsx';


function focusBoton (rutaString){
    return (window.location.pathname === rutaString) ? "admin-header-nav-activo" : null
}



export default function Header(){

    const navigate = useNavigate();

    const {cambiarTema} = useTema();

    return (
        <AppBar position="static" color="primary" sx={{ px: 2 }} >
            <Toolbar sx={{ justifyContent: 'space-between', display:'flex', flexWrap:'wrap' }}>
                {/* Logo */}
                <Typography
                    variant="h6"
                    component={Link}
                    to="/"
                    sx={{ textDecoration: 'none', fontWeight: 'bold', alignItems:'center', display: 'flex', gap: '8px' }}
                    className='cinzel-regular admin-header-logo'
                >
                <img src="/transparent-moebius-trefoil.svg" width={60}/> ArcanaCards
                </Typography>

                {/* Navigation */}
                <Box sx={{ display: 'flex', gap: 2, flexWrap: "wrap" }}>
                
                    <Button color="secondary" onClick={cambiarTema}>
                    <ContrastIcon/>
                    </Button>
                    
                    <Button component={Link} to={rutas.admin.usuarios} color="inherit" className={focusBoton(rutas.admin.usuarios)}>
                    Usuarios
                    </Button>
                    
                    <Button component={Link} to={rutas.admin.torneos} color="inherit" className={focusBoton(rutas.admin.torneos)}>
                    Torneos
                    </Button>
                    
                    <Button component={Link} to={rutas.admin.partidas} color="inherit" className={focusBoton(rutas.admin.partidas)}>
                    Partidas
                    </Button>

                    <Button onClick={()=>{hacerLogout(navigate)}} color="info" sx={{backgroundColor:"#000"}}>
                    Cerrar sesión
                    </Button>

                </Box>

            </Toolbar>
        </AppBar>
    )

}
