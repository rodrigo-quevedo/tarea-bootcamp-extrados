
import { Box, Typography, Container } from '@mui/material';


export default function Footer(){

    return (
        <Box
            component="footer"//elemento HTML
            sx={{ //styles
                py: 0.6,
                // mt: "100vh",
                // position: 'fixed', bottom: 0,
                width:"100%",
                backgroundColor: "#ccc"
            }}
        >
            <Container maxWidth="sm">
                <Typography variant="subtitle2" color="textPrimary" align="center">
                {'© '}
                {new Date().getFullYear()} Torneo de Cartas Coleccionables - Rodrigo Martín Quevedo
                </Typography>
            </Container>
        </Box>
    )
}