﻿using DAO.DTOs_en_DAOs.DatosUsuario;

namespace Trabajo_Final.Services.UsuarioServices.Buscar
{
    public interface IBuscarUsuarioService
    {
        //busqueda para admins
        public Task<IEnumerable<DatosCompletosUsuarioDTO>> BuscarDatosCompletosUsuarios();

        public Task<DatosCompletosUsuarioDTO> BuscarDatosCompletosUsuario(
            int id_logeado, string rol_logeado, int id_usuario);
        public Task<PerfilUsuarioDTO> BuscarPerfilUsuario(
            int id_logeado, string rol_logeado, int id_usuario);

    }
}
