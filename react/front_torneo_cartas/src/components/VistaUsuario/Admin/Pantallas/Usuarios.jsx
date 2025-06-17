
import { Box, Button, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper, Typography, Dialog, DialogTitle, DialogActions,DialogContent, Grid, Divider, Avatar, backdropClasses, TextField } from "@mui/material"

import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import InfoIcon from '@mui/icons-material/Info';
import MoreVertIcon from '@mui/icons-material/MoreVert';

import CachedIcon from '@mui/icons-material/Cached';

import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward';
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward';

import "./Usuarios.css"

import {useState, useEffect} from 'react'
import persistirAxiosHeaders from "../../../../utils/persistirAxiosHeaders";
import traerUsuarios from "../../../../services/traerUsuarios";
import { useNavigate } from "react-router";

import { rutas } from "../../../../config/rutas";

import enviarEliminarUsuario from "../../../../services/enviarEliminarUsuario"


export default function Usuarios(){

    const navigate = useNavigate();

    persistirAxiosHeaders()

    const [actionsPopupUsuario, setActionsPopupUsuario] = useState(null);
    const [detallePopupUsuario, setDetallePopupUsuario] = useState(null);
    const [editarPopupUsuario, setEditarPopupUsuario] = useState(null);
    const [eliminarUsuario, setEliminarUsuario] = useState(null);

    // useEffect(()=>{
    //     console.log(detallePopupUsuario)
    // }, [detallePopupUsuario])

    const [usuarios, setUsuarios] = useState(null)
    const [usuariosMostrados, setUsuariosMostrados] = useState(null)

    useEffect(()=>{
        traerUsuarios(setUsuarios, setUsuariosMostrados)
    }, [])


    //ordenar por ID
    const [ordenAscendente, setOrdenAscendente] = useState(false);
    
    function ordenarUsuariosPorId(){
        const usuariosOrdenados = usuariosMostrados?.sort((a, b) =>
            ordenAscendente ? a.id - b.id : b.id - a.id
        );

        setUsuariosMostrados(usuariosOrdenados);
        setOrdenAscendente(!ordenAscendente);
    };

    //filtrar por nombre de usuario
    const [terminoBusqueda, setTerminoBusqueda] = useState('');

    function manejarBusqueda(e) {
        if (e.key !== "Enter") return;

        const resultado =  usuarios?.filter(
            (usuario) =>
                usuario?.nombre_apellido?.toLowerCase().trim()
                .includes(
                    e.target.value?.toLowerCase().trim()
                )
        );
        setUsuariosMostrados(resultado);
    };


    return (
        <Box sx={{ px: {sm:4},py:4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>

            <Typography variant="h1" gutterBottom fontSize={30} paddingBottom={1} sx={{alignSelf:"start"}}>
                Gestión de usuarios
            </Typography>


            {(usuarios === null) ?
            
            // mensaje de loading 
            <div className="grow-pulse-animation loading-container" >
                <Typography className="medievalsharp-regular" sx={{fontSize:30}}>
                    Buscando lista de usuarios...
                </Typography>
                
                <CachedIcon sx={{fontSize: 40}} className="rotate-animation"/>
           
            </div>
            :
            <>

            

            <Box sx={{display: 'flex', flexWrap: 'wrap', justifyContent: 'center', alignItems: 'end', gap: 2, mb: 2}} >
                <Button variant="contained" sx={{mb:{xs:0, sm:2}}} onClick={()=>navigate(rutas.admin.crearUsuario)}>
                    + Crear usuario
                </Button>

                <Button onClick={ordenarUsuariosPorId} variant="outlined" color="primary" sx={{mb:{xs:0, sm:2}}}>
                    ID {ordenAscendente ? <ArrowDownwardIcon /> : <ArrowUpwardIcon />}
                </Button>
                
                <TextField
                    // fullWidth
                    label="Buscar por nombre"
                    variant="outlined"
                    value={terminoBusqueda}
                    onChange={(e)=>setTerminoBusqueda(e.target.value)}
                    onKeyDown={manejarBusqueda}
                    margin="normal"
                />

            </Box>


            <TableContainer component={Paper} sx={{ maxWidth: 800, width: '100%', boxShadow: "1px 1px 10px #111"}} >
                <Table className="admin-usuariosTable" >
                    <TableHead className="cinzel cinzel-bold" sx={{
                        backgroundColor: theme => theme.palette.primary.main
                    }}>
                        <TableRow >
                            <TableCell align="center"><strong>ID</strong></TableCell>
                            <TableCell align="center"><strong>Rol</strong></TableCell>
                            <TableCell align="center" ><strong>Nombre y apellido</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Email</strong></TableCell>
                            {/* <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Detalle usuario</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Editar usuario</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Eliminar usuario</strong></TableCell> */}
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong></strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-mobile-col"><strong></strong></TableCell>
                        </TableRow>
                    </TableHead>

                    <TableBody className="jura">
                        {usuariosMostrados.map((usuario) => (
                            <TableRow key={usuario.id}>
                                <TableCell  align="center">{usuario.id}</TableCell>

                                <TableCell  align="center">{usuario.rol}</TableCell>

                                <TableCell  align="center" >{usuario.nombre_apellido}</TableCell>

                                <TableCell  align="center" className="admin-usuariosTable-desktop-col">{usuario.email}</TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col admin-usuariosTableActions">
                                    <Button variant="outlined" size="small" color="secondary"  onMouseDown={()=>{setDetallePopupUsuario(usuario)}}><InfoIcon /></Button>

                                     <Button variant="outlined" size="small" color="secondary"><EditIcon /></Button>

                                    <Button variant="outlined" size="small" color="primary" onClick={()=>setEliminarUsuario(usuario)}><DeleteIcon /></Button>
                                </TableCell>

                                <TableCell align="center" className="admin-usuariosTable-mobile-col">
                                    <Button variant="outlined" size="small"  color="primary" className=" admin-usuariosTable-actionButton" onMouseDown={()=>{setActionsPopupUsuario(usuario)}}>
                                        <MoreVertIcon />
                                    </Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            </>
            }

            {/* Popup acciones usuario */}
            <Dialog 
                open={actionsPopupUsuario !== null} 
                onClose={()=>{setActionsPopupUsuario(null)}}
                // slots={{ transition: null }}
                disableAutoFocus disableEnforceFocus
                slotProps={{paper:{style:{paddingTop:"8px", width: 300}}}}
            >
                                    
                <div className="admin-popupTitleID" >
                    <DialogTitle sx={{px:0}}>Acciones para Usuario</DialogTitle>
                    <strong style={{fontSize:"20px",border: "1px solid #000", padding: "6px"}}>
                        ID: {actionsPopupUsuario?.id}
                    </strong>
                </div>

                <DialogActions sx={{ flexDirection: 'column', alignItems: 'stretch', gap: 1, px: 3, pb: 3 }}>
                    <Button
                        variant="outlined"
                        startIcon={<InfoIcon />}
                        onClick={() => {
                            setActionsPopupUsuario(null)
                            for (const usuario of usuarios) {
                                if (usuario.id == actionsPopupUsuario?.id){
                                    setDetallePopupUsuario(usuario)
                                    break;
                                }     
                            }                            
                        }} >
                    Detalles
                    </Button>
                    <Button
                        variant="outlined"
                        color="primary"
                        startIcon={<EditIcon />}
                        onClick={() => {}} >
                    Editar
                    </Button>
                    <Button
                        variant="outlined"
                        color="error"
                        startIcon={<DeleteIcon />}
                        onClick={() => {setEliminarUsuario(actionsPopupUsuario)}} >
                    Eliminar
                    </Button>
               </DialogActions>
            </Dialog>

            {/* Detalles popup */}
            <Dialog 
                open={detallePopupUsuario !== null} 
                onClose={()=>{setDetallePopupUsuario(null); setActionsPopupUsuario(null)}}
                // slots={{ transition: null }}
                disableAutoFocus disableEnforceFocus
                sx={{ backgroundColor: 'rgba(0, 0, 0, 0.5)', backdropFilter: 'blur(4px)', pt: "8px"}}
                // disableScrollLock
                slotProps={{paper:{style:{paddingTop:"8px", width: 500, paddingBottom:"60px"}}}}
                >
                
                {/* <DialogTitle sx={{px:0}}>
                    Detalles del Usuario
                    <strong style={{border: "1px solid #000", padding: "6px"}}>
                    ID: {detallePopupUsuario?.id}
                    </strong>
                    </DialogTitle> */}
                

                <DialogContent sx={{pt: "8px"}}>

                    <div className="admin-popupTitleID" >
                        <DialogTitle sx={{px:0}}>Detalles del Usuario</DialogTitle>
                        <strong style={{fontSize:"20px",border: "1px solid #000", padding: "6px"}}>
                            ID: {detallePopupUsuario?.id}
                        </strong>
                    </div>
                        
                    <Typography className="admin-detalleUsuario-dato" sx={{backgroundColor: "#000", color: "#fff", textAlign: "center", py: 1.5}}>Email: <strong>{detallePopupUsuario?.email}</strong></Typography>                        

                    <Typography className="admin-detalleUsuario-dato" sx={{backgroundColor: "#ddd", color: "#000", textAlign: "center", py:0.7}}>Rol:  <strong>{detallePopupUsuario?.rol.toUpperCase()}</strong></Typography>                     

                    {(detallePopupUsuario?.rol !== "admin" && detallePopupUsuario?.rol !== "organizador") && 
                        <Avatar
                            src={detallePopupUsuario?.foto}
                            alt={detallePopupUsuario?.alias}
                            sx={{ width: 120, height: 120, my:2, mx: "auto" }}
                        />
                    }

                    <div style={{border: "1px solid #000"}}>
                        {(detallePopupUsuario?.rol !== "admin" && detallePopupUsuario?.rol !== "organizador") && 
                        <>
                        <Typography className="admin-detalleUsuario-dato">
                            <strong>Alias</strong> 
                            <br />
                            {(detallePopupUsuario?.alias)? 
                                detallePopupUsuario?.alias 
                                : 
                                <code style={{color:"#400"}}>Sin alias</code>}
                        </Typography> 
                        
                        <hr />
                        </>
                        }

                        <Typography className="admin-detalleUsuario-dato"><strong>Nombre y Apellido</strong> <br /> {detallePopupUsuario?.nombre_apellido}</Typography>

                        <hr />
                        
                        <Typography className="admin-detalleUsuario-dato"><strong>País</strong> <br />{detallePopupUsuario?.pais}</Typography>
                        
                        <hr />

                        <Typography className="admin-detalleUsuario-dato"><strong>Creado por Usuario ID</strong> <br /> {(detallePopupUsuario?.id_usuario_creador === 0)? "----": detallePopupUsuario?.id_usuario_creador}</Typography>
                    </div>

                </DialogContent>
            </Dialog>

             {/* Popup CONFIRMAR eliminar usuario */}
            <Dialog 
                open={eliminarUsuario !== null} 
                onClose={()=>{setEliminarUsuario(null)}}
                // slots={{ transition: null }}
                disableAutoFocus disableEnforceFocus
                slotProps={{paper:{style:{paddingTop:"8px", width: 500}}}}
            >
                                    
                <div className="admin-popupTitleID" >
                    <DialogTitle align="center" sx={{px:0}}>Seguro que quiere eliminar al usuario?</DialogTitle>
                    <strong style={{fontSize:"20px",border: "1px solid #000", padding: "6px"}}>
                        ID: {eliminarUsuario?.id}
                    </strong>
                </div>

                <DialogActions sx={{ flexDirection: 'column', alignItems: 'stretch', gap: 1, px: 3, pb: 3 }}>
                    <Button
                        variant="outlined"
                        color="secondary"
                        onMouseDown={() => {setEliminarUsuario(null)}} >
                    Cancelar
                    </Button>
                    <Button
                        variant="outlined"
                        color="primary"
                        startIcon={<DeleteIcon />}
                        onClick={()=>enviarEliminarUsuario(eliminarUsuario?.id, setEliminarUsuario)} >
                    Eliminar
                    </Button>
               </DialogActions>

                <DialogContent sx={{pt: "8px"}}>
                        
                    <Typography className="admin-detalleUsuario-dato" sx={{backgroundColor: "#000", color: "#fff", textAlign: "center", py: 1.5}}>Email: <strong>{eliminarUsuario?.email}</strong></Typography>                        

                    <Typography className="admin-detalleUsuario-dato" sx={{backgroundColor: "#ddd", color: "#000", textAlign: "center", py:0.7}}>Rol:  <strong>{eliminarUsuario?.rol.toUpperCase()}</strong></Typography>                     

                    {(eliminarUsuario?.rol !== "admin" && eliminarUsuario?.rol !== "organizador") && 
                        <Avatar
                            src={eliminarUsuario?.foto}
                            alt={eliminarUsuario?.alias}
                            sx={{ width: 120, height: 120, my:2, mx: "auto" }}
                        />
                    }

                    <div style={{border: "1px solid #000"}}>
                        {(eliminarUsuario?.rol !== "admin" && eliminarUsuario?.rol !== "organizador") && 
                        <>
                        <Typography className="admin-detalleUsuario-dato">
                            <strong>Alias</strong> 
                            <br />
                            {(eliminarUsuario?.alias)? 
                                eliminarUsuario?.alias 
                                : 
                                <code style={{color:"#400"}}>Sin alias</code>}
                        </Typography> 
                        
                        <hr />
                        </>
                        }

                        <Typography className="admin-detalleUsuario-dato"><strong>Nombre y Apellido</strong> <br /> {eliminarUsuario?.nombre_apellido}</Typography>

                        <hr />
                        
                        <Typography className="admin-detalleUsuario-dato"><strong>País</strong> <br />{eliminarUsuario?.pais}</Typography>
                        
                        <hr />

                        <Typography className="admin-detalleUsuario-dato"><strong>Creado por Usuario ID</strong> <br /> {(eliminarUsuario?.id_usuario_creador === 0)? "----": eliminarUsuario?.id_usuario_creador}</Typography>
                    </div>

                </DialogContent>

            </Dialog>

        </Box>
    )
}