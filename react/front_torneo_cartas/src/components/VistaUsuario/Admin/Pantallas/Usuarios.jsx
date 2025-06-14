
import { Box, Button, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper, Typography, Dialog, DialogTitle, DialogActions,DialogContent, Grid, Divider, Avatar, backdropClasses, TextField } from "@mui/material"

import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import InfoIcon from '@mui/icons-material/Info';
import MoreVertIcon from '@mui/icons-material/MoreVert';

import CachedIcon from '@mui/icons-material/Cached';

import "./Usuarios.css"

import {useState, useEffect} from 'react'
import persistirAxiosHeaders from "../../../../utils/persistirAxiosHeaders";
import actualizarUsuarios from "../../../../services/actualizarUsuarios";



export default function Usuarios(){

    persistirAxiosHeaders()

    const [actionsPopupUsuarioID, setActionsPopupUsuarioID] = useState(null);
    const [detallePopupUsuario, setDetallePopupUsuario] = useState(null);
    const [editarPopupUsuario, setEditarPopupUsuario] = useState(null);

    // useEffect(()=>{
    //     console.log(detallePopupUsuario)
    // }, [detallePopupUsuario])

    const [usuarios, setUsuarios] = useState(null)

    useEffect(()=>{
        actualizarUsuarios(setUsuarios)
    }, [])

    return (
        <Box sx={{ px: {sm:4},py:4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>

            <Typography variant="h1" gutterBottom fontSize={40} paddingBottom={6}>
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
            <Button variant="contained" sx={{ mb: 3 }}>
            + Crear usuario
            </Button>

            <TableContainer component={Paper} sx={{ maxWidth: 800, width: '100%'}} >
                <Table className="admin-usuariosTable">
                    <TableHead>
                        <TableRow>
                            <TableCell align="center"><strong>ID</strong></TableCell>
                            <TableCell align="center"><strong>Rol</strong></TableCell>
                            <TableCell align="center" ><strong>Nombre y apellido</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Email</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Detalle usuario</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Editar usuario</strong></TableCell>
                            <TableCell align="center" className="admin-usuariosTable-desktop-col"><strong>Eliminar usuario</strong></TableCell>
                            {/* <TableCell align="center" className="admin-usuariosTable-mobile-col"><strong>Acciones</strong></TableCell> */}
                        </TableRow>
                    </TableHead>

                    <TableBody className="jura">
                        {usuarios.map((usuario) => (
                            <TableRow key={usuario.id}>
                                <TableCell  align="center">{usuario.id}</TableCell>

                                <TableCell  align="center">{usuario.rol}</TableCell>

                                <TableCell  align="center" >{usuario.nombre_apellido}</TableCell>

                                <TableCell  align="center" className="admin-usuariosTable-desktop-col">{usuario.email}</TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col" onMouseDown={()=>{setDetallePopupUsuario(usuario)}}>
                                    <Button variant="outlined" size="small"><InfoIcon /></Button>
                                </TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col">
                                    <Button variant="outlined" size="small" color="primary"><EditIcon /></Button>
                                </TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col">
                                    <Button variant="outlined" size="small" color="error"><DeleteIcon /></Button>
                                </TableCell>

                                <TableCell align="center" >
                                    <Button variant="outlined" size="small"  color="primary" className="admin-usuariosTable-mobile-col admin-usuariosTable-actionButton" onMouseDown={()=>{setActionsPopupUsuarioID(usuario.id)}}>
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
                open={actionsPopupUsuarioID !== null} 
                onClose={()=>{setActionsPopupUsuarioID(null)}}
                // slots={{ transition: null }}
                disableAutoFocus disableEnforceFocus
                slotProps={{paper:{style:{paddingTop:"8px", width: 300}}}}
            >
                                    
                <div className="admin-popupTitleID" >
                    <DialogTitle sx={{px:0}}>Acciones para Usuario</DialogTitle>
                    <strong style={{fontSize:"20px",border: "1px solid #000", padding: "6px"}}>
                        ID: {actionsPopupUsuarioID}
                    </strong>
                </div>

                <DialogActions sx={{ flexDirection: 'column', alignItems: 'stretch', gap: 1, px: 3, pb: 3 }}>
                    <Button
                        variant="outlined"
                        startIcon={<InfoIcon />}
                        onClick={() => {
                            setActionsPopupUsuarioID(null)
                            for (const usuario of usuarios) {
                                if (usuario.id == actionsPopupUsuarioID){
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
                        onClick={() => {setActionsPopupUsuarioID(null)}} >
                    Editar
                    </Button>
                    <Button
                        variant="outlined"
                        color="error"
                        startIcon={<DeleteIcon />}
                        onClick={() => {setActionsPopupUsuarioID(null)}} >
                    Eliminar
                    </Button>
               </DialogActions>
            </Dialog>

            {/* Detalles popup */}
            <Dialog 
                open={detallePopupUsuario !== null} 
                onClose={()=>{setDetallePopupUsuario(null); setActionsPopupUsuarioID(null)}}
                // slots={{ transition: null }}
                disableAutoFocus disableEnforceFocus
                sx={{ backgroundColor: 'rgba(0, 0, 0, 0.5)', backdropFilter: 'blur(4px)', pt: "8px"}}
                // disableScrollLock
                slotProps={{paper:{style:{paddingTop:"8px", width: 350}}}}
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

                    <Avatar
                        src={detallePopupUsuario?.foto}
                        alt={detallePopupUsuario?.alias}
                        sx={{ width: 120, height: 120, my:2, mx: "auto" }}
                    />

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

        </Box>
    )
}