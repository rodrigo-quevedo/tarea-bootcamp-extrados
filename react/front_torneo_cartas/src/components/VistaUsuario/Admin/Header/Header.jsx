import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router';
import axios from 'axios';


export default function Header(){

    const navigate = useNavigate();

    const hacerLogout = () => {
        localStorage.removeItem('sesion');

        axios.defaults.headers.common['Authorization'] = null;

        navigate('/login');
    };

    return (
        <AppBar position="static" color="primary" sx={{ px: 2 }}>
            <Toolbar sx={{ justifyContent: 'space-between' }}>
                {/* Logo */}
                <Typography
                    variant="h6"
                    component={Link}
                    to="/"
                    
                    sx={{ textDecoration: 'none', color: 'inherit', fontWeight: 'bold' }}
                >
                Torneo de Cartas Coleccionables
                </Typography>

                {/* Navigation */}
                <Box sx={{ display: 'flex', gap: 2 }}>
                
                    <Button component={Link} to="/admin/usuarios" color="inherit">
                    Usuarios
                    </Button>
                    
                    <Button component={Link} to="/admin/torneos" color="inherit">
                    Torneos
                    </Button>
                    
                    <Button component={Link} to="/admin/partidas" color="inherit">
                    Partidas
                    </Button>

                    <Button onClick={hacerLogout} color="inherit">
                    Cerrar sesión
                    </Button>

                </Box>

            </Toolbar>
        </AppBar>
    )

}
