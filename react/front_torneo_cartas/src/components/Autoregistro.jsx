import Footer from "./Footer/Footer";

import {
    Box,
    Grid,
    Button,
    TextField,
    Typography,
    Paper,
    Stack,
} from '@mui/material';
import { Link } from 'react-router';

export default function Autoregistro(){
    return (
        <>
            <Box sx={{ minHeight: '100vh', backgroundColor: '#fff', color: "#000" }}>
        
               <Typography variant="h1" fontSize={30} textAlign={"center"} py={5}>Torneo de Cartas Coleccionables</Typography>
                    
        
                <Paper elevation={10} sx={{ p:  { xs: 2, sm: 3, md: 4 }, maxWidth:{ xs: '95%', sm: '80%', md: '60%' }, mx:"auto"}}>

                    <Typography variant="h5" gutterBottom>Registrar nuevo jugador :</Typography>

                    <Box component="form" noValidate autoComplete="off" onSubmit={(e)=>{e.preventDefault(); console.log("submitted a form")}}>
                        
                        <TextField
                            fullWidth
                            label="Email"
                            type="email"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Password"
                            type="password"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Confirmar Password"
                            type="password"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Pais"
                            type="text"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Nombre y Apellido"
                            type="text"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="URL foto perfil"
                            type="url"
                            margin="normal"
                            required
                        />
                        <TextField
                            fullWidth
                            label="Alias"
                            type="text"
                            margin="normal"
                            required
                        />
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            fullWidth
                            sx={{ mt: 2 }}
                        >
                            Registrarse
                        </Button>
                    </Box>

                </Paper>
        
                <Paper elevation={3} sx={{ my: 3, px: 3, py:2, textAlign: 'center', display: 'block', maxWidth: 300, mx:"auto"}}>
                    <Typography variant="body2" fontSize={15} gutterBottom>Si ya tienes usuario, inicia sesión: </Typography>
                    <Button
                        component={Link}
                        to="/login"
                        variant="contained"
                        color="primary"
                        fullWidth
                    >
                        Iniciar Sesión
                    </Button>
                </Paper> 
    
            </Box>
        
            <Footer />
        </>
    )
}