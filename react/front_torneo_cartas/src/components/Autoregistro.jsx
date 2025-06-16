import Footer from "./Footer/Footer";

import { Box, Button, TextField, Typography, Paper, Autocomplete } from '@mui/material';
import { Link } from 'react-router';

import { useNavigate } from "react-router";

import revisarSesionAbierta from "../utils/revisarSesionAbierta";

import paisesYZonasHorarias from "../config/paisesYZonasHorarias"
import { useState } from "react";


const validateEmail = (value, setEmail, setError) => {
    setEmail(value);
    // Simple email regex
    // const isValid = /^+$/.test(value);
    // setError(!isValid);
};


export default function Autoregistro(){

    const [email, setEmail] = useState();
    const [error, setError] = useState(false);

    const navigate = useNavigate()

    revisarSesionAbierta(navigate)

    const [pais, setPais] = useState(null);

    return (
        <>
            <Box sx={{ minHeight: '100vh', backgroundColor: '#fff', color: "#000" }}>
        
               <Typography variant="h1" fontSize={30} textAlign={"center"} py={5}>Torneo de Cartas Coleccionables</Typography>
                    
        
                <Paper elevation={10} sx={{ p:  { xs: 2, sm: 3, md: 4 }, maxWidth:{ xs: '95%', sm: '80%', md: '60%' }, mx:"auto"}}>

                    <Typography variant="h5" gutterBottom>Registrar nuevo jugador :</Typography>

                    <Box component="form" 
                        noValidate 
                        autoComplete="off" 
                        onSubmit={(e)=>{e.preventDefault(); console.log("submitted a form")}}
                    >
                        
                        <TextField
                            fullWidth
                            label="Email"
                            type="email"
                            margin="normal"
                            required
                            // value={email}
                            // onChange={(e) => validateEmail(e.target.value, setEmail, setError)}
                            // error={error}
                            // helperText={error ? 'Por favor, ingrese un email válido.' : ''}
                        />
                        <TextField
                            fullWidth
                            label="Nombre y Apellido"
                            type="text"
                            margin="normal"
                            required
                        />
                        <Autocomplete
                            fullWidth
                            options={paisesYZonasHorarias}
                            value={pais}
                            onChange={(event, newValue) => {
                                setPais(newValue || '');
                            }}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label="País"
                                    margin="normal"
                                    required
                                />
                            )}
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