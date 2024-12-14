using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using tarea_API_Web_REST.Utils.Exceptions;

namespace tarea_API_Web_REST.Services.LibroServices
{
    public class VerificarDisponibilidadService
    {
        LibroDAO libroDAO { get; set; }
        public VerificarDisponibilidadService(string connectionString)
        {
            libroDAO = new LibroDAO(connectionString);
        }

        // ya se recibe un libro que existe (comprobar la existencia del libro es ejecutado por otro service)
        public Libro Verificar(Libro libro) {

            //chequear que no esté prestado

            if (libro.fechaHora_prestamo != null && libro.fechaHora_vencimiento != null && libro.username_prestatario != null)
            {
                DateTime fechaHora_prestamo_notNull = (DateTime)libro.fechaHora_prestamo;

                throw new NotAvaiableException(
                    $"El libro '{libro.titulo}' con id '{libro.id}' ya fue prestado a la fecha {fechaHora_prestamo_notNull.ToUniversalTime().ToString("yyyy-MM-ddTHH:mmZ")}."
                );
            }
            ;

            //devolver libro disponible
            return libro;

        }


    }
}
