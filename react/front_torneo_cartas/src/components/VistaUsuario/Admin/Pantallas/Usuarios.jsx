
import { Box, Button, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper, Typography, Dialog, DialogTitle, DialogActions } from "@mui/material"

import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import InfoIcon from '@mui/icons-material/Info';
import MoreVertIcon from '@mui/icons-material/MoreVert';

import "./Usuarios.css"

import {useState, useEffect} from 'react'
import persistirAxiosHeaders from "../../../../utils/persistirAxiosHeaders";
import actualizarUsuarios from "../../../../services/actualizarUsuarios";



export default function Usuarios(){

    persistirAxiosHeaders()

    const [actionsPopupUsuarioID, setActionsPopupUsuarioID] = useState(null);

    const [usuarios, setUsuarios] = useState([{
        "id": null,
        "foto": null,
        "alias": null,
        "nombre_apellido": null,
        "pais": null,
        "email": null,
        "activo": null,
        "id_usuario_creador": null
    }])

    useEffect(()=>{
        actualizarUsuarios(setUsuarios)
    }, [])

    return (
        <Box sx={{ px: {sm:4},py:4, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>

            <Typography variant="h1" gutterBottom fontSize={40} paddingBottom={6}>
                Gestión de usuarios
            </Typography>

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

                    <TableBody>
                        {usuarios.map((usuario) => (
                            <TableRow key={usuario.id}>
                                <TableCell  align="center">{usuario.id}</TableCell>

                                <TableCell  align="center">{usuario.rol}</TableCell>

                                <TableCell  align="center" >{usuario.nombre_apellido}</TableCell>

                                <TableCell  align="center" className="admin-usuariosTable-desktop-col">{usuario.email}</TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col" >
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

            <Dialog 
                open={actionsPopupUsuarioID !== null} 
                onClose={()=>{setActionsPopupUsuarioID(null)}}
                // slots={{ transition: null }}
                >
                <DialogTitle>Acciones para usuario ID: {actionsPopupUsuarioID}</DialogTitle>
                <DialogActions sx={{ flexDirection: 'column', alignItems: 'stretch', gap: 1, px: 3, pb: 3 }}>
                    <Button
                        variant="outlined"
                        startIcon={<InfoIcon />}
                        onClick={() => {}} >
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
                        onClick={() => {}} >
                    Eliminar
                    </Button>
               </DialogActions>
            </Dialog>
        </Box>
    )
}