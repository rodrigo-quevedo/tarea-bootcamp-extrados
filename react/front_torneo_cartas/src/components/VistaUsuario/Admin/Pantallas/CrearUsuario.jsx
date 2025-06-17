import Footer from "../../../Footer/Footer";

import { Box, Button, TextField, Typography, Paper, Autocomplete } from '@mui/material';
import { Link } from 'react-router';

import { useNavigate } from "react-router";

import revisarSesionAbierta from "../../../../utils/revisarSesionAbierta";

import paisesYZonasHorarias from "../../../../config/paisesYZonasHorarias"
import { useState } from "react";

import validarPais, { validarAlias, validarConfirmPassword, validarEmail, validarFotoURL, validarNombreApellido, validarPassword, validarRol } from "../../../../utils/formValidations";
import userFormProperties from "../../../../config/userFormProperties";

import crearUsuario from '../../../../services/crearUsuario.js'
import { rutas } from "../../../../config/rutas";

import { ArrowBack } from "@mui/icons-material";
import roles from "../../../../config/roles";


export default function CrearUsuario(){

    const [error, setError] = useState({});

    const [email, setEmail] = useState(null);
    const [password, setPassword] = useState(null);
    const [confirmPassword, setConfirmPassword] = useState(null);
    const [rol, setRol] = useState(null);
    const [pais, setPais] = useState(null);
    const [nombre_apellido, setNombreApellido] = useState(null);
    const [foto, setFoto] = useState(null);
    const [alias, setAlias] = useState(null);
    

    const navigate = useNavigate()
    
    //error condition
    let errorCondition = {
        [userFormProperties.rol]: error[userFormProperties.rol] !== undefined && error[userFormProperties.rol]!== null,

        [userFormProperties.email]:  error[userFormProperties.email] !== undefined && error[userFormProperties.email] !== null,

        [userFormProperties.nombre_apellido]:  error[userFormProperties.nombre_apellido] !== undefined && error[userFormProperties.nombre_apellido] !== null,

        [userFormProperties.pais]:  error[userFormProperties.pais] !== undefined && error[userFormProperties.pais] !== null,

        [userFormProperties.foto]:  error[userFormProperties.foto] !== undefined && error[userFormProperties.foto] !== null,

        [userFormProperties.alias]:  error[userFormProperties.alias] !== undefined && error[userFormProperties.alias] !== null,

        [userFormProperties.password]:  error[userFormProperties.password] !== undefined && error[userFormProperties.password] !== null,

        [userFormProperties.confirmPassword]:  error[userFormProperties.confirmPassword] !== undefined && error[userFormProperties.confirmPassword] !== null,
        
    } 

    console.log("error adentro:", error)

    return (
        <>
            <Box sx={{ px: {sm:4},py:4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        
                

                <Typography variant="h1" gutterBottom fontSize={30} paddingBottom={1} sx={{alignSelf:"start"}}>
                    <Button
                        variant="outlined"
                        color="primary"
                        startIcon={<ArrowBack />}
                        onClick={() => navigate(rutas.admin.usuarios)}
                        sx={{mr:2}}
                    >
                    Volver
                    </Button>
                    Gestión de usuarios
                </Typography>
                    
        
                <Paper elevation={10} sx={{ p:  { xs: 2, sm: 3, md: 4 }, maxWidth:{ xs: '95%', sm: '80%', md: '60%' }, mx:"auto"}}>

                    <Typography variant="h5" gutterBottom>Crear Usuario</Typography>

                    <Box component="form" 
                        noValidate 
                        autoComplete="off" 
                        onSubmit={(e)=> {
                            e.preventDefault();

                            //verificar que todos los campos son validos
                            if (rol !== "admin" && rol !== "organizador"){
                                validarFotoURL(foto, setFoto, error, setError, true)
                                validarAlias(alias, setAlias, error, setError, true)
                            }

                            // if (
                                validarRol(rol, setRol, error, setError, true) 
                                validarEmail(email, setEmail, error, setError, true) 
                                validarNombreApellido(nombre_apellido, setNombreApellido, error, setError, true)
                                validarPais(pais, setPais, error, setError, true) 
                                validarPassword(password, setPassword, error, setError, true) 
                                validarConfirmPassword(password, confirmPassword, setConfirmPassword, error, setError, true)
                            // )   
                                //hacer request
                                crearUsuario({rol, email, nombre_apellido, pais, foto, alias, password, confirmPassword}, navigate)}}
                    >
                        

                        <Autocomplete
                            fullWidth
                            options={roles}
                            value={rol}
                            onChange={(event, newValue)=>validarRol(newValue, setRol, error, setError, true)}
                            error={errorCondition[userFormProperties.rol]}
                            
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label="Rol"
                                    margin="normal"
                                />
                            )}
                        />
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

                       {(rol === "admin" || rol === "organizador")? null:
                       <>
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
                        </>
                        }
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
                            Crear
                        </Button>
                    </Box>

                </Paper>
    
            </Box>
        </>
    )
}