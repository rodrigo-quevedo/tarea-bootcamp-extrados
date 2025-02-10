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
using Trabajo_Final.utils.Exceptions.Exceptions;



namespace DAO.DAOs.UsuarioDao
{
    public class UsuarioDAO : IUsuarioDAO
    {
        // Connection
        private MySqlConnection connection { get; set; }


        public UsuarioDAO(string connectionString)
        {
            connection = new SingletonConnection(connectionString).Instance;
        }

        // ------------------ CRUD ------------------ //

        public async Task<int> CrearUsuario(Usuario usuario)
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

        public async Task<int> CrearUsuario(Usuario usuario, int id_usuario_creador)
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
        public Usuario BuscarUsuarios(Usuario usuario)
        {
            return null;
        }

        //READ
        public async Task<Usuario> BuscarUnUsuario(Usuario usuario) // Búsqueda por id, email, rol (este ultimo para Verificar_Existencia_Admin)
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
        public async Task<int> GuardarRefreshToken(int id, string refreshToken)
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

        public async Task<int> BorradoLogicoRefreshToken(int id, string refreshToken)
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

        public async Task<Refresh_Token> BuscarRefreshToken(string refreshToken, bool activo)
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


    }
}
