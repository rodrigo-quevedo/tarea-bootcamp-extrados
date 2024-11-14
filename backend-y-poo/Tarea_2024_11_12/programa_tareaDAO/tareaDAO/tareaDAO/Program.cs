// See https://aka.ms/new-console-template for more information
using tareaDAO_libreria_de_clases.DAO;

Console.WriteLine("Tarea DAO");

UsuarioDAO usuarioDAO = new UsuarioDAO();

//READ: Lista de usuarios
Console.WriteLine("Lista de usuarios:");
var listaUsuarios = usuarioDAO.read_lista_usuarios();
foreach(var usuario in listaUsuarios)
{
    usuario.mostrarDatos();
}

//READ: Usuario por ID
