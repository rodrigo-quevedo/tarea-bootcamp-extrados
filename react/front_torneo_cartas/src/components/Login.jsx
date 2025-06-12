import Footer from "./Footer/Footer";

import { Box, Grid, Button, TextField, Typography, Paper, Stack } from '@mui/material';
import { Link } from 'react-router';
import { useState } from "react";
import {cuentasDemo} from "../config/cuentasDemo";
import manejoLogin from "../services/manejoLogin";
import { useNavigate } from 'react-router';

import revisarSesionAbierta from "../utils/revisarSesionAbierta";



export default function Login(){

    const navigate = useNavigate();

    revisarSesionAbierta(navigate)


    let demoButtonStyles = {color: "#fff", background: "#000"}

    const [datosUsuario, setDatosUsuario] = useState({email: "", password: ""}) 

    return (
    <>
        <Box sx={{ minHeight: '100vh', backgroundColor: '#fff', color: "#000" }}>

            <Typography variant="h1" fontSize={30} textAlign={"center"} py={5}>Torneo de Cartas Coleccionables</Typography>
            

            <Paper elevation={3} sx={{ py: 4, px:{xs: 2, sm: 0}, maxWidth:{xs: "90%", md:"75%"}, mx:"auto", transition: 'all 0.3s',
    '&:hover': { boxShadow: 13, scale: "1.003"}}}>
                <Grid container spacing={6} justifyContent="center" alignItems="flex-start">

                    {/* Columna 1: login form */}                
                    <Grid item >
                        <Typography variant="h5" gutterBottom>Iniciar sesión</Typography>

                        <Box component="form" noValidate autoComplete="off" onSubmit={(e)=>{manejoLogin(e, datosUsuario, navigate)}}>
                            
                            <TextField
                                fullWidth
                                label="Email"
                                type="email"
                                margin="normal"
                                value={datosUsuario.email}
                                onChange={(e)=>{setDatosUsuario({...datosUsuario, email: e.target.value})}}
                                required
                            />
                            <TextField
                                fullWidth
                                label="Password"
                                type="password"
                                margin="normal"
                                value={datosUsuario.password}
                                onChange={(e)=>{setDatosUsuario({...datosUsuario, password: e.target.value})}}
                                required
                            />
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                sx={{ mt: 2 }}
                            >
                                Ingresar
                            </Button>
                        </Box>
                    </Grid>

                    {/* Columna 2: cuentas demo */}
                    <Grid item sx={{backgroundColor:"#000", color: "#fff", p:3, borderRadius: '5px'}}>
                            <Typography variant="h6" gutterBottom pb={2}>Cuentas Demo</Typography>

                            <Stack spacing={2}>
                                <Button variant="outlined" fullWidth sx={demoButtonStyles} onClick={()=>{setDatosUsuario(cuentasDemo.Admin); console.log(cuentasDemo.Admin)}}>Admin</Button>

                                <Button variant="outlined" fullWidth onClick={()=>setDatosUsuario(cuentasDemo.Organizador)}sx={demoButtonStyles}>Organizador</Button>
                                
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}onClick={()=>setDatosUsuario(cuentasDemo.Juez)}>Juez</Button>
                                
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}onClick={()=>setDatosUsuario(cuentasDemo.Jugador)}>Jugador</Button>
                            </Stack>
                    </Grid>

                </Grid>
            </Paper>

            {/* Abajo del login: registro */}
            <Paper elevation={3} sx={{ my: 3, px: 3, py:2, textAlign: 'center', display: 'block', maxWidth: 300, mx:"auto"}}>
                <Typography variant="body2" fontSize={15} gutterBottom>Registro para Nuevos Jugadores</Typography>
                <Button
                    component={Link}
                    to="/registro"
                    variant="contained"
                    color="primary"
                    fullWidth
                >
                    Nuevo jugador
                </Button>
            </Paper>

        </Box>

        <Footer />
    </>
    )
}