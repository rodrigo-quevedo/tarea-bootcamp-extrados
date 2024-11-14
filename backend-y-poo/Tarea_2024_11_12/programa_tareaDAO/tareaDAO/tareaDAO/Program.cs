// See https://aka.ms/new-console-template for more information
using tareaDAO_libreria_de_clases.DAO;
using tareaDAO_libreria_de_clases.Entidades;

Console.WriteLine("Tarea DAO");

UsuarioDAO usuarioDAO = new UsuarioDAO();

prueba_READ_lista_usuarios();
prueba_READ_usuario_by_id(1);
prueba_CREATE_usuario(10, "cristian", 30);
prueba_UPDATE_usuario(1, "julian", 50);
prueba_DELETE_borrado_logico_usuario(1);




//READ: Lista de usuarios
void prueba_READ_lista_usuarios()
{
    IEnumerable<Usuario> listaUsuarios = usuarioDAO.read_lista_usuarios();

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
}


//READ: Usuario por ID
void prueba_READ_usuario_by_id(int idPrueba)
{
    Usuario usuarioById = usuarioDAO.read_usuario_by_id(idPrueba);

    Console.WriteLine($"Mostrar usuario con Id={idPrueba}:");
    if (usuarioById != null)
    {
        usuarioById.mostrarDatos();
    }
    else {
        Console.WriteLine("null");
    }
}


//CREATE usuario
void prueba_CREATE_usuario(int idPruebaCreate, string nombrePruebaCreate, int edadPruebaCreate)
{
    Usuario usuarioCreado = usuarioDAO.create_usuario(idPruebaCreate, nombrePruebaCreate, edadPruebaCreate);

    Console.WriteLine($"Mostrar usuario creado con id {idPruebaCreate}:");
    if (usuarioCreado != null)
    {
        usuarioCreado.mostrarDatos();
    }
    else
    {
        Console.WriteLine("null");
    }
}


//UPDATE usuario
void prueba_UPDATE_usuario(int idPruebaUpdate, string nuevoNombre, int nuevaEdad)
{
    Usuario usuarioModificado = usuarioDAO.update_usuario(idPruebaUpdate, nuevoNombre, nuevaEdad);

    Console.WriteLine($"Usuario id={idPruebaUpdate} luego del update:");
    if (usuarioModificado != null)
    {
        usuarioModificado.mostrarDatos();
    }
    else
    {
        Console.WriteLine("null");
    }
}

//DELETE usuario (borrado logico)
void prueba_DELETE_borrado_logico_usuario(int idPruebaUpdate)
{
    Usuario usuarioModificado = usuarioDAO.delete_borrado_logico_usuario(idPruebaUpdate);

    Console.WriteLine($"Usuario id={idPruebaUpdate} luego del borrado logico:");
    if (usuarioModificado != null)
    {
        usuarioModificado.mostrarDatos();
    }
    else
    {
        Console.WriteLine("null");
    }
}
