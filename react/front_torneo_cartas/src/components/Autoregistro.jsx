import Footer from "./Footer/Footer";

import { Box, Button, TextField, Typography, Paper, Autocomplete } from '@mui/material';
import { Link } from 'react-router';

import { useNavigate } from "react-router";

import revisarSesionAbierta from "../utils/revisarSesionAbierta";

import paisesYZonasHorarias from "../config/paisesYZonasHorarias"
import { useState } from "react";

import validarPais, { validarAlias, validarConfirmPassword, validarEmail, validarFotoURL, validarNombreApellido, validarPassword } from "../utils/formValidations";
import userFormProperties from "../config/userFormProperties";

import envioAutoregistro from "../services/envioAutoregistro"


export default function Autoregistro(){

    const [error, setError] = useState({});

    const [email, setEmail] = useState(null);
    const [nombre_apellido, setNombreApellido] = useState(null);
    const [pais, setPais] = useState(null);
    const [foto, setFoto] = useState(null);
    const [alias, setAlias] = useState(null);
    const [password, setPassword] = useState(null);
    const [confirmPassword, setConfirmPassword] = useState(null);
    

    const navigate = useNavigate()
    
    revisarSesionAbierta(navigate)
    
    //error condition
    let errorCondition = {
        [userFormProperties.email]:  error[userFormProperties.email] !== undefined && error[userFormProperties.email] !== null,

        [userFormProperties.nombre_apellido]:  error[userFormProperties.nombre_apellido] !== undefined && error[userFormProperties.nombre_apellido] !== null,

        [userFormProperties.pais]:  error[userFormProperties.pais] !== undefined && error[userFormProperties.pais] !== null,

        [userFormProperties.foto]:  error[userFormProperties.foto] !== undefined && error[userFormProperties.foto] !== null,

        [userFormProperties.alias]:  error[userFormProperties.alias] !== undefined && error[userFormProperties.alias] !== null,

        [userFormProperties.password]:  error[userFormProperties.password] !== undefined && error[userFormProperties.password] !== null,

        [userFormProperties.confirmPassword]:  error[userFormProperties.confirmPassword] !== undefined && error[userFormProperties.confirmPassword] !== null,
        
    } 

    

    return (
        <>
            <Box sx={{ minHeight: '100vh', backgroundColor: '#fff', color: "#000" }}>
        
               <Typography variant="h1" fontSize={30} textAlign={"center"} py={5}>Torneo de Cartas Coleccionables</Typography>
                    
        
                <Paper elevation={10} sx={{ p:  { xs: 2, sm: 3, md: 4 }, maxWidth:{ xs: '95%', sm: '80%', md: '60%' }, mx:"auto"}}>

                    <Typography variant="h5" gutterBottom>Registrar nuevo jugador :</Typography>

                    <Box component="form" 
                        noValidate 
                        autoComplete="off" 
                        onSubmit={(e)=> {
                            e.preventDefault();

                            //verificar que todos los campos son validos
                            validarEmail(email, setEmail, error, setError, true)
                            validarNombreApellido(nombre_apellido, setNombreApellido, error, setError, true)
                            validarPais(pais, setPais, error, setError, true)
                            validarFotoURL(foto, setFoto, error, setError, true)
                            validarAlias(alias, setAlias, error, setError, true)
                            validarPassword(password, setPassword, error, setError, true)
                            validarConfirmPassword(password, confirmPassword, setConfirmPassword, error, setError, true)
                            
                            //hacer request
                            envioAutoregistro({email, nombre_apellido, pais, foto, alias, password, confirmPassword}, navigate)}}
                    >
                        
                        <TextField
                            fullWidth
                            label="Email"
                            type="email"
                            margin="normal"
                            value={email}
                            onChange={(e) => validarEmail(e.target.value, setEmail, error, setError, true)}
                            error={errorCondition[userFormProperties.email]}
                            helperText={error ? error[userFormProperties.email] : ''}
                        />
                        <TextField
                            fullWidth
                            label="Nombre y Apellido"
                            type="text"
                            margin="normal"
                            onChange={(e) => validarNombreApellido(e.target.value, setNombreApellido, error, setError, true)}
                            error={errorCondition[userFormProperties.nombre_apellido]}
                            helperText={error ? error[userFormProperties.nombre_apellido] : ''}

                        />
                        <Autocomplete
                            fullWidth
                            options={paisesYZonasHorarias}
                            value={pais}
                            onChange={(event, newValue)=>validarPais(newValue, setPais, error, setError, true)}
                            error={errorCondition[userFormProperties.nombre_apellido]}
                            // helperText={error ? error[userFormProperties.nombre_apellido] : ''}
                            
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
                             onChange={(e) => validarFotoURL(e.target.value, setFoto, error, setError, true)}
                            error={errorCondition[userFormProperties.foto]}
                            helperText={error ? error[userFormProperties.foto] : ''}
                        />
                        <TextField
                            fullWidth
                            label="Alias"
                            type="text"
                            margin="normal"
                            onChange={(e) => validarAlias(e.target.value, setAlias, error, setError, true)}
                            error={errorCondition[userFormProperties.alias]}
                            helperText={error ? error[userFormProperties.alias] : ''}
                        />
                        <TextField
                            fullWidth
                            label="Password"
                            type="password"
                            margin="normal"
                            onChange={(e) => validarPassword(e.target.value, setPassword, error, setError, true)}
                            error={errorCondition[userFormProperties.password]}
                            helperText={error ? error[userFormProperties.password] : ''}
                        />
                        <TextField
                            fullWidth
                            label="Confirmar Password"
                            type="password"
                            margin="normal"
                            onChange={(e) => validarConfirmPassword(password, e.target.value, setConfirmPassword, error, setError, true)}
                            error={errorCondition[userFormProperties.confirmPassword]}
                            helperText={error ? error[userFormProperties.confirmPassword] : ''}
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