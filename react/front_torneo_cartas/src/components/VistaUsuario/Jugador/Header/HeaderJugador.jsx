import { AppBar, Toolbar, Typography, Button, Box} from '@mui/material';
import { Link, useNavigate } from 'react-router';

import ContrastIcon from '@mui/icons-material/Contrast';

import "./Header.css"

import {rutas} from "../../../../config/rutas.js"
import hacerLogout from '../../../../utils/hacerLogout.jsx';
import {useTema} from '../../../../hooks/useCambiarTheme.jsx';


function focusBoton (rutaString){
    return (window.location.pathname === rutaString) ? "admin-header-nav-activo" : null
}



export default function HeaderJugador(){

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
                    className='cinzel admin-header-logo'
                >
                <img src="/transparent-moebius-trefoil.svg" width={60}/> ArcanaCards
                </Typography>

                {/* Navigation */}
                <Box sx={{ display: 'flex', gap: 2, flexWrap: "wrap" }}>
                
                    <Button component={Link} to={rutas.jugador.coleccion} color="inherit" className={focusBoton(rutas.jugador.coleccion)}>
                    Coleccion
                    </Button>
                    
                    <Button component={Link} to={rutas.jugador.mazo} color="inherit" className={focusBoton(rutas.jugador.mazo)}>
                    Mazo
                    </Button>
                    

                    <Button color="secondary" onClick={cambiarTema} variant='contained' sx={{p:0}}>
                        <ContrastIcon/>
                    </Button>

                    <Button 
                        color="secondary" 
                        variant='outlined' 
                        onClick={()=>{hacerLogout(navigate)}}
                        sx={{
                            backgroundColor: theme => theme.palette.secondary.contrastText,
                            color: theme => theme.palette.secondary.main,
                            '&:hover': {
                                backgroundColor: theme => theme.palette.secondary.dark,
                                color:  theme => theme.palette.secondary.contrastText,
                            }
                        }}
                    >
                        Cerrar sesión
                    </Button>

                </Box>

            </Toolbar>
        </AppBar>
    )

}
