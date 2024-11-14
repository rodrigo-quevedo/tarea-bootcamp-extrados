﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using tareaDAO_libreria_de_clases.Entidades;

namespace tareaDAO_libreria_de_clases.DAO
{
    public class UsuarioDAO
    {
        //CREATE
        public void create()
        {

        }

        //READ (lista usuarios)
        public IEnumerable<Usuario> read_lista_usuarios()
        {
            string query = "SELECT * FROM Usuarios;";
            //esta string, o el user y password, se deberia guardar y traer de una variable de entorno
            //(ahora no importa porque es una DB de practica).

            //-> documentacion de la connection string para MySQLConnector: https://mysqlconnector.net/connection-options/
            string connection_string = "Server=localhost;Port=3306;Username=tareaDAO_user;Password=123456;Database=tarea_dao;";
           
            try
            {
                using (var connection = new MySqlConnection(connection_string))
                {
                    //Console.WriteLine(connection.State);
                    
                    var lista =  connection.Query<Usuario>(query).ToList();

                    return lista;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        //READ (info de un usuario a partir de su Id)


    
        //UPDATE


        //DELETE (borrado logico)

    }
}
