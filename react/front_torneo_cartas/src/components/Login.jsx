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



export default function Login(){

    let demoButtonStyles = {color: "#fff", background: "#000"}
    // let demoButtonStyles = {}

    return (
    <>
        <Box sx={{ minHeight: '100vh', backgroundColor: '#fff', color: "#000" }}>

            <Typography variant="h1" fontSize={30} textAlign={"center"} py={5}>Torneo de Cartas Coleccionables</Typography>
            

            <Paper elevation={3} sx={{ py: 4, maxWidth:"90%", mx:"auto", transition: 'all 0.3s',
    '&:hover': { boxShadow: 10, scale: "1.005", backgroundColor: "#eff"}}}>
                <Grid container spacing={1} justifyContent="space-around" alignItems="flex-start">

                    {/* Column 2: Login Form */}                
                    <Grid item xs={12} md={8}>
                        <Typography variant="h5" gutterBottom>Iniciar sesión</Typography>

                        <Box component="form" noValidate autoComplete="off">
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
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                sx={{ mt: 2 }}
                            >
                                Sign In
                            </Button>
                        </Box>
                    </Grid>

                    {/* Column 3: Demo Login Buttons */}
                    <Grid item xs={12} md={3} sx={{backgroundColor:"#ccc", color: "#000", p:3, borderRadius: '5px'}}>
                            <Typography variant="h6" gutterBottom pb={2}>Cuentas Demo</Typography>

                            <Stack spacing={2}>
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}>Admin</Button>
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}>Organizador</Button>
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}>Juez</Button>
                                <Button variant="outlined" fullWidth sx={demoButtonStyles}>Jugador</Button>
                            </Stack>
                    </Grid>

                </Grid>
            </Paper>

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