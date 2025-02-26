using Custom_Exceptions.Exceptions.Exceptions;
using DAO.Connection;
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

        public int CrearUsuario(Usuario usuario)
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

        public async Task<int> CrearUsuarioAsync(Usuario usuario)
        {
            string insertQuery = "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo) " +
                "VALUES(@Rol, @Pais, @Nombre_apellido, @Email, @Password, @Activo);";

            try
            {
                return await connection.ExecuteAsync(insertQuery, new
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


        public async Task<int> CrearUsuarioAsync(Usuario usuario, int id_usuario_creador)
        {
            string insertQuery = "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo, id_usuario_creador) " +
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
                                 " AND token_activo = @Activo ";

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
                new {id_usuario, alias});

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
                    Id_usuario = id_usuario});

            //id_usuario no existe: Solo si un Admin elimina al usuario antes que corra esto.

            Console.WriteLine($"UPSERT /perfil result: {result}");

            if (result == 0) throw new Exception(exceptionMessage);

            return successMessage;
        }




    }
}
