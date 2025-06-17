
import {Box, Typography, Grid, Card, Chip, CardMedia, CardContent} from "@mui/material"

import { Cached } from "@mui/icons-material";
import { useEffect, useState } from "react";
import traerCartasColeccionadas from "../../../../services/traerCartasColeccionadas";

import colorDeSeries from "../../../../config/colorDeSeries";

export default function Mazo(){

    const [cartasColeccionadas, setCartasColeccionadas] = useState(null)


    useEffect(()=>{
        traerCartasColeccionadas(setCartasColeccionadas)
    },[])

    return(
        <Box sx={{ px: {sm:4},py:4, alignItems: 'center' }}>

            <Typography variant="h1" gutterBottom fontSize={30} paddingBottom={5} sx={{alignSelf:"start"}}>
                Cartas coleccionadas
            </Typography>
        

        {(cartasColeccionadas === null|| cartasColeccionadas === undefined) ?
            
            // mensaje de loading 
            <div className="grow-pulse-animation loading-container" >
                <Typography className="medievalsharp-regular" sx={{fontSize:30}}>
                    Buscando cartas...
                </Typography>
                
                <Cached sx={{fontSize: 40}} className="rotate-animation"/>
           
            </div>
            :
            <Box component="form">



                <Grid container spacing={3} justifyContent="center" className="cinzel cinzel-bold">
                    {cartasColeccionadas.map((carta) => (
                    
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
        
        </Box>

    );
}