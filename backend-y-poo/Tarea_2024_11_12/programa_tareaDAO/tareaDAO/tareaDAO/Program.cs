// See https://aka.ms/new-console-template for more information
using tareaDAO_libreria_de_clases.DAO;

Console.WriteLine("Tarea DAO");

UsuarioDAO usuarioDAO = new UsuarioDAO();

//READ: Lista de usuarios
var listaUsuarios = usuarioDAO.read_lista_usuarios();
    //prueba:
Console.WriteLine("Lista de usuarios:");
if (listaUsuarios != null)
{
    foreach(var usuario in listaUsuarios)
    {
        usuario.mostrarDatos();
    }
}
else
{
    Console.WriteLine("null");
}

//READ: Usuario por ID
int idPrueba = 1;
var usuarioById = usuarioDAO.read_usuario_by_id(idPrueba);
    //prueba:
Console.WriteLine($"Mostrar usuario con Id={idPrueba}:");
if (usuarioById != null)
{
    usuarioById.mostrarDatos();
}
else {
    Console.WriteLine("null");
}


//CREATE usuario
int idPruebaCreate = 5;
string nombrePruebaCreate = "juan ignacio";
int edadPruebaCreate = 100;
var usuarioCreado = usuarioDAO.create_usuario(idPruebaCreate, nombrePruebaCreate, edadPruebaCreate);
    //prueba
Console.WriteLine($"Mostrar usuario creado con id {idPruebaCreate}:");
if (usuarioCreado != null)
{
    usuarioCreado.mostrarDatos();
}
else
{
    Console.WriteLine("null");
}

