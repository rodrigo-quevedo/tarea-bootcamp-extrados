import { useState, useEffect } from "react";
import traerCartas from "../../../../services/traerCartas";

import { Box, Typography, Grid, Card, CardMedia, CardContent, Chip, Button, Dialog, DialogTitle, DialogContent, DialogActions } from "@mui/material";

import Cached from '@mui/icons-material/Cached'
import colorDeSeries from "../../../../config/colorDeSeries";
import verificarYGuardarMazo from "../../../../utils/verificarYGuardarMazo";

export default function Coleccion(){

    const [cartas, setCartas] = useState(null)

    useEffect(()=>{
        traerCartas(setCartas)
    }, [])

    const [cartasSeleccionadas, setCartasSeleccionadas] = useState([])

    const [popupError, setPopupError] = useState("");
    const [popupExito, setPopupExito] = useState(false);
    

    return(
        <Box sx={{py:4, alignItems: 'center' }}>

            <Typography variant="h1" gutterBottom fontSize={30} paddingBottom={5} sx={{alignSelf:"start", px: {sm:4}}}>
                Coleccionar cartas
            </Typography>
        

        {(cartas === null|| cartas === undefined) ?
            
            // mensaje de loading 
            <Box className="grow-pulse-animation loading-container" sx={{mx: {sm:4}}}>
                <Typography className="medievalsharp-regular" sx={{fontSize:30}}>
                    Buscando cartas...
                </Typography>
                
                <Cached sx={{fontSize: 40}} className="rotate-animation"/>
           
            </Box>
            :
            <Box component="form" width="100vw">

                
                <Typography gutterBottom fontSize={25} paddingBottom={5} 
                    sx={{
                        px: {sm:4},
                        alignItems:"center",
                        alignSelf: "start",
                        position: "sticky",
                        top: 0,
                        backgroundColor: theme => theme.palette.background.default,
                        color: theme => theme.palette.text.primary,
                        zIndex: 1,
                        py: 2}}
                >
                    Seleccionadas 
                    <strong style={{color: "#fff", background: "#6A0000", margin: "0 5px"}}> {cartasSeleccionadas.length} </strong> 
                    de 
                    <strong style={{color: "#6A0000"}}> 15 </strong>cartas 

                    <Button variant="contained" sx={{ml:{sm: 3}}} onMouseDown={()=>{verificarYGuardarMazo(cartasSeleccionadas, setPopupError, setPopupExito)}}>
                    📚 Guardar mazo
                    </Button>

                    <Button variant="contained" sx={{ml:{sm: 3}}} onMouseDown={()=>{setCartasSeleccionadas([])}}>
                    ♻️ Reiniciar
                    </Button>

                </Typography>


                <Grid container spacing={3} justifyContent="center" width="85%" className="cinzel cinzel-bold">
                    {cartas.map((carta) => (
                    
                        <Grid item mt={1} xs={12} sm={6} md={4} lg={3} key={carta.id} >
                            <Card 
                                sx={{ 
                                    maxWidth: 345, mx: 'auto', p:1, 
                                    border:"1px solid #6A0000",
                                    boxShadow:"1px 1px 10px #000",
                                    ...(carta.series.length === 1? 
                                        { backgroundColor: colorDeSeries(carta.series) }
                                        : 
                                        { backgroundImage: colorDeSeries(carta.series) }) 
                                }}
                                onMouseDown={(e)=>{
                                    console.log(carta.id, ' presionada')
                                    setCartasSeleccionadas(prev => 
                                    prev.includes(carta.id) ? 
                                        prev.filter(cartaId => cartaId !== carta.id) //eliminar
                                        : 
                                        [...prev, carta.id] //agregar
                                    );
                                }}
                                className={cartasSeleccionadas.includes(carta.id) ? "carta-coleccion carta-seleccionada" : "carta-coleccion"}
                        >
                                
                                <Grid container spacing={0.5} sx={{ my: 1, justifyContent:"space-between", color: "#000" }}>
                                    Series: 
                                    <div style={{display: "flex", gap: 2}}>
                                    {carta.series?.map((serie, idx) => (
                                        <Grid item key={idx}>
                                            <Chip label={serie} size="small" color="primary" />
                                        </Grid>
                                    ))}
                                    </div>
                                </Grid>                                
                                
                                <CardMedia
                                    component="img"
                                    height="180"
                                    image={carta.ilustracion}
                                    alt={`Carta ${carta.id}`}
                                    sx={{mb:2, borderRadius: "5px"}}
                                />
                                
                                <CardContent sx={{textAlign:"center", color: "#000"}} className="background-brillante mover-background">
                                    <Typography variant="h6" gutterBottom>
                                        ID {carta.id}
                                    </Typography>
                                    
                                    <Typography variant="body2" color="primary" 
                                        sx={{fontWeight:700}}
                                    >
                                        ATK: {carta.ataque} | DEF: {carta.defensa}
                                    </Typography>
                                    

                                </CardContent>
                            </Card>
                        </Grid>
                        ))}
                    </Grid>
            </Box>
            }
        
            {/* Popup Error */}
            <Dialog 
                open={Boolean(popupError)} onClose={() => setPopupError("")}
            >
                
                <DialogTitle sx={{fontSize:40, color: 'red'}}>Error</DialogTitle>
                <DialogContent>
                <Typography sx={{fontSize: 20}}>{popupError}</Typography>
                </DialogContent>
                <DialogActions>
                <Button onClick={() => setPopupError("")} autoFocus>
                    Cerrar
                </Button>
                </DialogActions>
            </Dialog>

            {/* Popup Éxito */}
            <Dialog open={popupExito} onClose={() => setPopupExito(false)}>
                <DialogTitle>Éxito</DialogTitle>
                <DialogContent>
                <Typography>
                    {(popupExito.length === true) ?
                        "El mazo ha sido guardado correctamente."
                        :
                        popupExito
                    }
                </Typography>
                </DialogContent>
                <DialogActions>
                <Button onClick={() => setPopupExito(false)} autoFocus>
                    Cerrar
                </Button>
                </DialogActions>
            </Dialog>


        </Box>

    );
}