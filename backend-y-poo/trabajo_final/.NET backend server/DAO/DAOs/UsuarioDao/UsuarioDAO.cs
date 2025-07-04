﻿using Constantes.Constantes;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
using DAO.DTOs_en_DAOs.DatosUsuario;
using DAO.DTOs_en_DAOs.EditarUsuario;
using DAO.DTOs_en_DAOs.RegistroUsuario;
using DAO.Entidades.PartidaEntidades;
using DAO.Entidades.UsuarioEntidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace DAO.DAOs.UsuarioDao
{
    public class UsuarioDAO : IUsuarioDAO
    {
        // Connection
        private MySqlConnection connection { get; set; }


        public UsuarioDAO(string connectionString)
        {
            connection = new SingletonConnection(connectionString).Instance;
            if (connection.State != System.Data.ConnectionState.Open) connection.Open();
        }

        // ------------------ CRUD ------------------ //

        public int CrearPrimerAdmin_Sync(Usuario usuario)
        {
            string insertQuery = "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo) " +
                "VALUES(@Rol, @Pais, @Nombre_apellido, @Email, @Password, @Activo);";

            try
            {
                return connection.Execute(insertQuery, new
                {
                    usuario.Rol,
                    usuario.Pais,
                    usuario.Nombre_apellido,
                    usuario.Email,
                    usuario.Password, //recibe password YA ENCRIPTADA
                    usuario.Activo
                });
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Duplicate entry")
                    &&
                    e.Message.Contains("for key 'usuarios.email'")
                )
                    throw new AlreadyExistsException($"El usuario con mail [{usuario.Email}] ya existe.");

                throw e;
            }
        }

        public async Task<bool> CrearUsuarioAsync(DatosRegistroUsuarioDTO dto)
        {
            //por defecto no hay id_usuario_creador
            string usuarioInsertQuery =
                " INSERT INTO usuarios " +
                "   (rol, pais, nombre_apellido, email, password, activo) " +
                " VALUES " +
                "   (@rol, @pais, @nombre_apellido, @email, @password, @activo); ";



            if (dto.id_usuario_creador != default) usuarioInsertQuery =
                " INSERT INTO usuarios " +
                "   (rol, pais, nombre_apellido, email, password, activo, id_usuario_creador) " +
                " VALUES " +
                "   (@rol, @pais, @nombre_apellido, @email, @password, @activo, @id_usuario_creador); ";

            string perfilInsertQuery = " INSERT INTO perfil_usuarios (id_usuario, foto, alias) " +
                                       " VALUES (@id_usuario, @foto, @alias); ";



            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int usuariosResult = await connection.ExecuteAsync(usuarioInsertQuery, dto, transaction);

                    int id_usuario = await connection.QueryFirstAsync<int>(
                        " SELECT LAST_INSERT_ID(); ",
                        null,
                        transaction);

                    int perfilResult;
                    if (dto.alias == default && dto.foto == default) perfilResult = 1;
                    else perfilResult = await connection.ExecuteAsync(
                        perfilInsertQuery,
                        new { id_usuario, dto.foto, dto.alias },
                        transaction);

                    if (usuariosResult == 0 || perfilResult == 0) throw new DefaultException($"No se pudo crear el usuario [{dto.email}].");

                    transaction.Commit();
                }
                catch (MySqlException e)
                {
                    transaction.Rollback();

                    if (e.Message.Contains("Duplicate entry") && e.Message.Contains("for key 'usuarios.email'"))
                        throw new AlreadyExistsException($"El usuario con mail [{dto.email}] ya existe.");

                    throw e;
                }
            }

            return true;
        }


        public async Task<int> CrearUsuarioAsync(Usuario usuario, int id_usuario_creador)
        {
            string insertQuery =
                "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo, id_usuario_creador) " +
                "VALUES(@Rol, @Pais, @Nombre_apellido, @Email, @Password, @Activo, @Id_usuario_creador);";

            try
            {
                return await connection.ExecuteAsync(insertQuery, new
                {
                    usuario.Rol,
                    usuario.Pais,
                    usuario.Nombre_apellido,
                    usuario.Email,
                    usuario.Password, //recibe password YA ENCRIPTADA
                    usuario.Activo,
                    Id_usuario_creador = id_usuario_creador
                });
            }
            catch (MySqlException e)
            {
                if (e.Message.Contains("Duplicate entry")
                    &&
                    e.Message.Contains("for key 'usuarios.email'")
                )
                    throw new AlreadyExistsException($"El usuario con mail [{usuario.Email}] ya existe.");

                throw e;
            }


        }


        //READ
        public async Task<IEnumerable<int>> BuscarIDsUsuarios(Usuario busqueda)
        {
            string selectQuery;
            if (busqueda.Rol != default)
            {
                selectQuery = " SELECT id FROM usuarios " +
                              " WHERE rol = @Rol AND activo = @Activo; ";

                return await connection.QueryAsync<int>(selectQuery, new
                {
                    Rol = busqueda.Rol,
                    Activo = busqueda.Activo
                });
            }
            //else if (busqueda.Pais != default) { }
            //else if (busqueda.Nombre_apellido != default) { }

            throw new Exception("Debe ingresar un rol para buscar los usuarios.");
        }


        public Usuario BuscarUnUsuario(Usuario usuario) // Búsqueda por id, email, rol (este ultimo para Verificar_Existencia_Admin)
        {
            string selectQuery;

            if (usuario.Id != default) //default de int es 0
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE id=@Id AND activo=@Activo;";

                return connection.QueryFirstOrDefault<Usuario>(selectQuery, new
                {
                    usuario.Id,
                    usuario.Activo
                });
            }

            else if (usuario.Email != default) //default de string es null
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE email=@Email AND activo=@Activo;";

                return connection.QueryFirstOrDefault<Usuario>(selectQuery, new
                {
                    usuario.Email,
                    usuario.Activo
                });
            }

            else if (usuario.Rol != default) //default de string es null
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE rol=@rol AND activo=@activo;";

                return connection.QueryFirstOrDefault<Usuario>(selectQuery, new
                {
                    rol = usuario.Rol,
                    activo = usuario.Activo
                });
            }


            //si no se cumple ninguno de esos parámetros, devuelve null
            Console.WriteLine("No se pasó ningún id, email o rol para la búsqueda, se devolverá null");
            return null;

        }


        public async Task<Usuario> BuscarUnUsuarioAsync(Usuario usuario) // Búsqueda por id, email, rol (este ultimo para Verificar_Existencia_Admin)
        {
            string selectQuery;

            if (usuario.Id != default) //default de int es 0
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE id=@Id AND activo=@Activo;";

                return await connection.QueryFirstOrDefaultAsync<Usuario>(selectQuery, new
                {
                    usuario.Id,
                    usuario.Activo
                });
            }

            else if (usuario.Email != default) //default de string es null
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE email=@Email AND activo=@Activo;";

                return await connection.QueryFirstOrDefaultAsync<Usuario>(selectQuery, new
                {
                    usuario.Email,
                    usuario.Activo
                });
            }

            else if (usuario.Rol != default) //default de string es null
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE rol=@rol AND activo=@activo;";

                return await connection.QueryFirstOrDefaultAsync<Usuario>(selectQuery, new
                {
                    rol = usuario.Rol,
                    activo = usuario.Activo
                });
            }


            //si no se cumple ninguno de esos parámetros, devuelve null
            Console.WriteLine("No se pasó ningún id, email o rol para la búsqueda, se devolverá null");
            return null;

        }



        //INSERT (refreshtoken)
        public async Task<int> GuardarRefreshTokenAsync(int id, string refreshToken)
        {
            string insertQuery = "INSERT INTO refresh_tokens(refresh_token, id_usuario, token_activo)  " +
                                 "VALUES (@Refresh_token, @Id, @Token_activo);";

            return await connection.ExecuteAsync(insertQuery, new
            {
                Refresh_token = refreshToken,
                Id = id,
                Token_activo = true
            });

        }

        //UPDATE (refreshToken): borrado logico

        public async Task<int> BorradoLogicoRefreshTokenAsync(int id, string refreshToken)
        {
            string updateQuery = "UPDATE refresh_tokens " +
                                 "SET token_activo = @Token_activo " +
                                 "WHERE id_usuario = @Id AND refresh_token = @Refresh_token;";

            return await connection.ExecuteAsync(updateQuery, new
            {
                Id = id,
                Refresh_token = refreshToken,
                Token_activo = false
            });
        }

        public async Task<Refresh_Token> BuscarRefreshTokenAsync(string refreshToken, bool activo)
        {
            //No hace falta buscar por id_usuario ya que está incluido en el JWT.
            //-> Para cada id_usuario y TIMESTAMP habrá un JWT único.

            string selectQuery = "SELECT * FROM refresh_tokens " +
                                 " WHERE refresh_token = @RefreshToken " +
                                 " AND token_activo = @Activo " +
                                 " AND " +
                                 "      @Activo = " +
                                 "          (SELECT activo from usuarios" +
                                 "           WHERE refresh_tokens.id_usuario = usuarios.id)";

            return await connection.QueryFirstOrDefaultAsync<Refresh_Token>(selectQuery, new
            {
                RefreshToken = refreshToken,
                Activo = activo
            });
        }

        public async Task<bool> VerificarAliasExistente(int id_usuario, string alias)
        {
            string selectQuery = " SELECT COUNT(*) FROM perfil_usuarios " +
                                 " WHERE alias = @alias " +
                                 " AND id_usuario != @id_usuario; ";

            int result = await connection.QueryFirstOrDefaultAsync<int>(selectQuery,
                new { id_usuario, alias });

            Console.WriteLine($"Verificar alias result: {result}");

            if (result > 0) throw new InvalidInputException($"El alias '{alias}' ya existe.");

            return true;
        }

        public async Task<string> ActualizarPerfil(int id_usuario, string url_foto, string alias)
        {
            //default: url_foto != null && alias != null

            string upsertQuery = " INSERT INTO perfil_usuarios " +
                                 " VALUES (@Id_usuario, @Foto, @Alias) " +

                                 " ON DUPLICATE KEY " +
                                 "      UPDATE foto = @Foto, alias = @Alias;";

            string exceptionMessage = $"No se pudo agregar la foto ni el alias del usuario [{id_usuario}]";
            string successMessage = $"Se agregó la foto y el alias del usuario [{id_usuario}] con éxito.";


            if (url_foto == null)
            {
                upsertQuery = " INSERT INTO perfil_usuarios " +
                              " VALUES (@Id_usuario, NULL, @Alias) " +

                              " ON DUPLICATE KEY " +
                              "      UPDATE alias = @Alias;";

                exceptionMessage = $"No se pudo agregar el alias del usuario [{id_usuario}]";
                successMessage = $"Se agregó el alias del usuario [{id_usuario}] con éxito.";

            }

            else if (alias == null)
            {
                upsertQuery = " INSERT INTO perfil_usuarios " +
                              " VALUES (@Id_usuario, @Foto, NULL) " +

                              " ON DUPLICATE KEY " +
                              "      UPDATE foto = @Foto;";

                exceptionMessage = $"No se pudo agregar la foto del usuario [{id_usuario}]";
                successMessage = $"Se agregó la foto del usuario [{id_usuario}] con éxito.";
            }

            //Las variables sobrantes no se usan, ya que cambia el query string.
            int result = await connection.ExecuteAsync(
                upsertQuery, new {
                    Foto = url_foto,
                    Alias = alias,
                    Id_usuario = id_usuario });

            //id_usuario no existe: Solo si un Admin elimina al usuario antes que corra esto.

            Console.WriteLine($"UPSERT /perfil result: {result}");

            if (result == 0) throw new Exception(exceptionMessage);

            return successMessage;
        }


        public async Task<bool> BorradoLogicoUsuarioYSesionesActivas(int id_usuario)
        {
            string borradoLogicoUsuarioQuery = " UPDATE usuarios " +
                                               " SET activo = @activo " +
                                               " WHERE id = @id_usuario; ";

            string borradoLogicoSesionesQuery = " UPDATE refresh_tokens " +
                                                " SET token_activo = @activo " +
                                                " WHERE id_usuario = @id_usuario; ";


            int usuarioResult = await connection.ExecuteAsync(
                borradoLogicoUsuarioQuery,
                new { id_usuario, activo = false });

            if (usuarioResult == 0) throw new InvalidInputException($"El usuario [{id_usuario}] no existe.");

            await connection.ExecuteAsync(
                borradoLogicoSesionesQuery,
                new { id_usuario, activo = false });

            return true;
        }

        public async Task<DatosEditablesUsuarioDTO> BuscarDatosEditablesUsuario(int id_usuario)
        {
            string selectQuery =
                " SELECT * FROM usuarios " +
                " LEFT JOIN perfil_usuarios" +//LEFT join porque puede que el perfil esté vacio
                " ON usuarios.id = perfil_usuarios.id_usuario " +
                " WHERE usuarios.id = @id_usuario" +
                " AND activo = @activo; ";

            return await connection.QueryFirstOrDefaultAsync<DatosEditablesUsuarioDTO>(
                selectQuery,
                new { id_usuario, activo = true });
        }

        public async Task<bool> EditarUsuario(DatosEditablesUsuarioDTO dto, string[] rolesPerfil)
        {
            string usuarioUpdateQuery =
                " UPDATE usuarios " +
                " SET " +
                "   nombre_apellido = @Nombre_apellido, " +
                //"   rol = @Rol, " +
                "   pais = @Pais, " +
                //"   email = @Email, " +
                "   password = @Password " +
                " WHERE" +
                "   id = @Id; ";


            string perfilUpsertQuery =
                " INSERT INTO perfil_usuarios " +
                " VALUES (@Id, @Foto, @Alias) " +

                " ON DUPLICATE KEY " +
                "      UPDATE foto = @Foto, alias = @Alias;";

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int usuarioQueryResult = await connection.ExecuteAsync(
                        usuarioUpdateQuery,
                        dto,
                        transaction);


                    //Organizador y admin no tienen perfil, no hace falta el UPDATE para esos casos
                    int perfilQueryResult;

                    if (!rolesPerfil.Contains(dto.Rol)) perfilQueryResult = 1;

                    else perfilQueryResult = await connection.ExecuteAsync(
                        perfilUpsertQuery,
                        dto,
                        transaction);


                    if (usuarioQueryResult == 0 || perfilQueryResult == 0) throw new Exception($"No se pudo editar el usuario [{dto.Id}].");

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();


                    throw ex;
                }
            }

            return true;
        }

        public async Task<DatosCompletosUsuarioDTO> BuscarDatosCompletosUsuario(
            int id_logeado, string rol_logueado, int id_usuario)
        {
            //default: admin query
            string selectQuery =
                " SELECT * FROM usuarios " +
                //LEFT join porque los roles 'admin' u 'organizador' no tienen perfil
                " LEFT JOIN perfil_usuarios " +
                " ON usuarios.id = perfil_usuarios.id_usuario " +
                " WHERE usuarios.id = @id_usuario; ";


            if (rol_logueado == Roles.ORGANIZADOR) 
                selectQuery =
                    " SELECT * FROM usuarios " +
                    " LEFT JOIN perfil_usuarios " +
                    " ON usuarios.id = perfil_usuarios.id_usuario " +

                    " WHERE usuarios.id = @id_usuario " +
                        
                    " AND ( " +
                    $"   (usuarios.rol = {Roles.JUEZ})" +//todos los jueces
                    "    OR  " +
                            //jugadores inscriptos a torneos de ese organizador
                    $"       (usuarios.rol = {Roles.JUGADOR}" +
                    "       AND" +
                    "       usuarios.id IN " +
                    "           (SELECT id_jugador FROM jugadores_inscriptos " +
                    "            WHERE jugadores_inscriptos.id_torneo IN " +
                    "               (SELECT torneos.id FROM torneos WHERE id_organizador = @id_logeado) " +
                    "           ) " +
                    "       )" +
                    " ); ";

          
            DatosCompletosUsuarioDTO result =
                await connection.QueryFirstOrDefaultAsync<DatosCompletosUsuarioDTO>(
                    selectQuery,
                    new { id_usuario, id_logeado });

            if (result == null) throw new InvalidInputException($"No se pudo encontrar los datos del usuario [{id_usuario}] por alguna de estas razones: 1. El usuario no existe. 2. No tiene permiso para ver los datos del usuario.");

            return result;

        }

        //Busqueda para Admins
        public async Task<IEnumerable<DatosCompletosUsuarioDTO>> BuscarDatosCompletosUsuarios()
        {
            //default: admin query
            string selectQuery =
                " SELECT * FROM usuarios " +
                //LEFT join porque los roles 'admin' u 'organizador' no tienen perfil
                " LEFT JOIN perfil_usuarios " +
                " ON usuarios.id = perfil_usuarios.id_usuario " +
                " WHERE usuarios.activo = @Activo; ";



            IEnumerable<DatosCompletosUsuarioDTO> result =
                await connection.QueryAsync<DatosCompletosUsuarioDTO>(
                    selectQuery,
                    new { Activo = true });

            if (result == null) throw new Exception($"No se encontraron usuarios en la base de datos.");

            return result;

        }


        public async Task<PerfilUsuarioDTO> BuscarPerfilUsuarioDTO(
            int id_logeado, string rol_logueado, int id_usuario)
        {
            //default: juez query
            string selectQuery =
                " SELECT foto, alias, id_usuario AS Id FROM perfil_usuarios " +
                " WHERE id_usuario = @id_usuario " +
                " AND ( " +
                "   id_usuario IN " +
                "       (SELECT id_jugador FROM jugadores_inscriptos " +
                "        WHERE jugadores_inscriptos.id_torneo IN " +
                "            (SELECT jueces_torneo.id_torneo FROM jueces_torneo " +
                "             WHERE jueces_torneo.id_juez = @id_logeado) " +
                "       ) " +
                " ); ";

            if (rol_logueado == Roles.JUGADOR)
                selectQuery =
                    " SELECT foto, alias, id_usuario AS id FROM perfil_usuarios " +
                    " WHERE id_usuario = @id_usuario " +
                    " AND( " +
                    "   id_usuario IN " +
                    "       (SELECT id_jugador_1 FROM partidas " +
                    "        WHERE id_jugador_1 = @id_logeado OR id_jugador_2 = @id_logeado) " +
                    "   OR " +
                    "   id_usuario IN " +
                    "       (SELECT id_jugador_2 FROM partidas " +
                    "       WHERE id_jugador_1 = @id_logeado OR id_jugador_2 = @id_logeado) " +
                    "   OR " +
                    "   id_usuario IN " +
                    "   (SELECT id_juez FROM partidas " +
                    "    WHERE id_jugador_1 = @id_logeado OR id_jugador_2 = @id_logeado)" +
                    " ); " ;


            PerfilUsuarioDTO result =
                await connection.QueryFirstOrDefaultAsync<PerfilUsuarioDTO>(
                    selectQuery,
                    new { id_usuario, id_logeado });

            if (result == null) throw new InvalidInputException($"No se pudo encontrar los datos del usuario [{id_usuario}] por alguna de estas razones: 1. El usuario no existe. 2. No tiene permiso para ver los datos del usuario.");

            return result;
        }




    }
}
