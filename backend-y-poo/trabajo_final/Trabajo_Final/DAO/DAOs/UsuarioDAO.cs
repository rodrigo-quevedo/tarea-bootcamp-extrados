using DAO.Connection;
using DAO.DAOs.DI;
using DAO.Entidades;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAO.DAOs
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

        public int  CrearUsuario(Usuario usuario)
        {
            string insertQuery = "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo) " +
                "VALUES(@Rol, @Pais, @Nombre_apellido, @Email, @Password, @Activo);";

            return connection.Execute(insertQuery, new
            {
                Rol = usuario.Rol,
                Pais = usuario.Pais,
                Nombre_apellido = usuario.Nombre_apellido,
                Email = usuario.Email,
                Password = usuario.Password, //recibe password YA ENCRIPTADA
                Activo = usuario.Activo
            });

           
        }

        public int CrearUsuario(Usuario usuario, int id_usuario_creador)
        {
            string insertQuery = "INSERT INTO usuarios(rol, pais, nombre_apellido, email, password, activo, id_usuario_creador) " +
                "VALUES(@Rol, @Pais, @Nombre_apellido, @Email, @Password, @Activo, @Id_usuario_creador);";

            return connection.Execute(insertQuery, new
            {
                Rol = usuario.Rol,
                Pais = usuario.Pais,
                Nombre_apellido = usuario.Nombre_apellido,
                Email = usuario.Email,
                Password = usuario.Password, //recibe password YA ENCRIPTADA
                Activo = usuario.Activo,
                Id_usuario_creador = id_usuario_creador
            });


        }

        //READ
        public Usuario BuscarUsuarios(Usuario usuario)
        {
            return null;
        }

        //READ
        public Usuario BuscarUnUsuario(Usuario usuario) // Búsqueda por id, email, rol (este ultimo para Verificar_Existencia_Admin)
        {
            string selectQuery;

            if (usuario.Id != default) //default de int es 0
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE id=@Id AND activo=@Activo;";

                return connection.QueryFirstOrDefault<Usuario>(selectQuery, new
                {
                    Id = usuario.Id,
                    Activo = usuario.Activo
                });
            }

            else if (usuario.Email != default) //default de string es null
            {
                selectQuery = "SELECT * FROM usuarios " +
                              "WHERE email=@Email AND activo=@Activo;";

                return connection.QueryFirstOrDefault<Usuario>(selectQuery, new
                {
                    Email = usuario.Email,
                    Activo = usuario.Activo
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



        //INSERT (refreshtoken)
        public int GuardarRefreshToken(int id, string refreshToken)
        {
            string insertQuery = "INSERT INTO refresh_tokens VALUES " +
                                 "(@Id, @Refresh_token, @Token_activo);";

            return connection.Execute(insertQuery, new
            {
                Id = id,
                Refresh_token = refreshToken,
                Token_activo = true
            });

        }

        //UPDATE (refreshToken): borrado logico

        public int BorradoLogicoRefreshToken(int id, string refreshToken)
        {
            string updateQuery = "UPDATE refresh_tokens " +
                                 "SET token_activo = @Token_activo" +
                                 "WHERE id = @Id AND refresh_token = @Refresh_token;";

            return connection.Execute(updateQuery, new
            {
                Id = id,
                Refresh_token = refreshToken,
                Token_activo = false
            });
        }

        public Refresh_Token BuscarRefreshToken(string refreshToken, bool activo)
        {
            //No hace falta buscar por id_usuario ya que está incluido en el JWT.
            //-> Para cada id_usuario y TIMESTAMP habrá un JWT único.

            string selectQuery = "SELECT * FROM refresh_tokens" +
                                 "WHERE refresh_token = @RefreshToken" +
                                 "AND activo = @Activo";

            return connection.QueryFirstOrDefault<Refresh_Token>(selectQuery, new
            {
                RefreshToken = refreshToken,
                Activo = activo
            });
        }


    }
}
