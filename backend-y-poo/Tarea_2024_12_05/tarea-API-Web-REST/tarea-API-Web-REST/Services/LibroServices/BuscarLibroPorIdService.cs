using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.LibroServices
{
    public class BuscarLibroPorIdService
    {
        LibroDAO libroDAO { get; set; }
        public BuscarLibroPorIdService(string connectionString)
        {
            libroDAO = new LibroDAO(connectionString);
        }

        public Libro BuscarLibro(int id) { 
            Libro libroEncontrado = libroDAO.BuscarLibroPorId(id);
            
            if (libroEncontrado == null) throw new NotFoundException($"No se encontró el libro con id '{id}'.");
        
            libroEncontrado.mostrarDatos();

            return libroEncontrado;
        }

    }
}
