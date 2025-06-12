
import { Box, Button, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Paper, Typography, Dialog, DialogTitle, DialogActions } from "@mui/material"

import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import InfoIcon from '@mui/icons-material/Info';
import MoreVertIcon from '@mui/icons-material/MoreVert';

import "./Usuarios.css"

import {useState} from 'react'



export default function Usuarios(){

    const [actionsPopupUsuarioID, setActionsPopupUsuarioID] = useState(null);

    const users = [
        { Id: 1, Rol: 'Admin' },
        { Id: 2, Rol: 'Player' },
        { Id: 3, Rol: 'Organizer' }
    ];

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
                        {users.map(usuario => (
                            <TableRow key={usuario.id}>
                                <TableCell  align="center">{usuario.Id}</TableCell>

                                <TableCell  align="center">{usuario.Rol}</TableCell>

                                <TableCell  align="center" >{usuario.Nombre_apellido}</TableCell>

                                <TableCell  align="center" className="admin-usuariosTable-desktop-col">{usuario.Email}</TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col">
                                    <Button variant="outlined" size="small"><InfoIcon /></Button>
                                </TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col">
                                    <Button variant="outlined" size="small" color="primary"><EditIcon /></Button>
                                </TableCell>

                                <TableCell align="center" className="admin-usuariosTable-desktop-col">
                                    <Button variant="outlined" size="small" color="error"><DeleteIcon /></Button>
                                </TableCell>

                                <TableCell align="center" >
                                    <Button variant="outlined" size="small"  color="primary" onClick={()=>{setActionsPopupUsuarioID(usuario.Id)}} className="admin-usuariosTable-mobile-col admin-usuariosTable-actionButton" >
                                        <MoreVertIcon />
                                    </Button>
                                </TableCell>

                                <Dialog open={actionsPopupUsuarioID === usuario.Id} onClose={()=>{setActionsPopupUsuarioID(null)}}>
                                    <DialogTitle>Acciones para usuario ID: {usuario.Id}</DialogTitle>
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


                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    )
}