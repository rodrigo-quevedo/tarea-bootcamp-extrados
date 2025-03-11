using Configuration.FilesPathConfiguration;
using Configuration.ServerRoutes;
using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.UsuarioDao;
using DAO.DTOs_en_DAOs.EditarUsuario;
using Isopoh.Cryptography.Argon2;
using Trabajo_Final.DTO.Request.InputUsuarios;

namespace Trabajo_Final.Services.UsuarioServices.Editar
{
    public class EditarUsuarioService: IEditarUsuarioService
    {
        private IUsuarioDAO usuarioDAO;
        private IServerRoutesConfiguration serverRoutesConfig;
        private IFilesPathsConfigurations filesPathsConfig;

        public EditarUsuarioService(
            IUsuarioDAO usuarioDao,
            IServerRoutesConfiguration serverRoutes,
            IFilesPathsConfigurations filesPaths
        ){
            usuarioDAO = usuarioDao;
            serverRoutesConfig = serverRoutes;
            filesPathsConfig = filesPaths;
        }

        public async Task<bool> EditarUsuario(int id_usuario, EditarUsuarioDTO dto)
        {
            DatosEditablesUsuarioDTO datosUsuario = 
                await usuarioDAO.BuscarDatosEditablesUsuario(id_usuario);

            if (datosUsuario == null) throw new InvalidInputException($"El usuario [{id_usuario}] no existe o no está activo.");

            DatosEditablesUsuarioDTO objetoUpdate = 
                await PrepararObjetoUpdate(id_usuario, dto, datosUsuario);

            return await usuarioDAO.EditarUsuario(objetoUpdate, new string[2] {Roles.JUGADOR, Roles.JUEZ} );
        }


        private async Task<DatosEditablesUsuarioDTO> PrepararObjetoUpdate(
            int id_usuario,
            EditarUsuarioDTO dto, 
            DatosEditablesUsuarioDTO datosUsuario)
        {
            //usuario
            if (dto.Nombre_apellido != default) datosUsuario.Nombre_apellido = dto.Nombre_apellido;
            if (dto.Rol != default) datosUsuario.Rol = dto.Rol;
            if (dto.Pais != default) datosUsuario.Pais = dto.Pais;
            if (dto.Email != default) datosUsuario.Email = dto.Email;
            if (dto.Password != default) datosUsuario.Password = Argon2.Hash(dto.Password);

            //perfil
            if (dto.Foto != default)
            {
                string foto_path =
                  filesPathsConfig.GetFotoPerfilPath()
                  + @"\"
                  + id_usuario
                  + Path.GetExtension(dto.Foto.FileName);

                //Si la foto anterior tiene la misma extension, el archivo se sobreescribe
                using (var stream = new FileStream(foto_path, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(stream);
                }

                datosUsuario.Foto =
                    serverRoutesConfig.GetFotoPerfilRoute()
                    + "/"
                    + id_usuario
                    + Path.GetExtension(dto.Foto.FileName);
            }
            
            if (dto.Alias != default) datosUsuario.Alias = dto.Alias;

            return datosUsuario;
        }



    }
}
