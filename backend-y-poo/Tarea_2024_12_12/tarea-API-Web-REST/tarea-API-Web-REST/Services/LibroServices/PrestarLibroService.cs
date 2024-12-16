using DAO_biblioteca_de_cases.DAOs;
using System.Security.Cryptography;
using tarea_API_Web_REST.Utils.RequestBodyParams;

namespace tarea_API_Web_REST.Services.LibroServices
{
    public class PrestarLibroService
    {
        LibroDAO libroDAO { get; set; }
        public PrestarLibroService(string connectionString)
        {
            libroDAO = new LibroDAO(connectionString);
        }

        public void PrestarLibro(PrestamoLibro prestamo, DateTime fechaHora_prestamo)
        {
            //calcular plazo de 5 días para vencimiento
            DateTime fechaHora_vencimiento = fechaHora_prestamo.AddDays(5);

            int rowsAffected = libroDAO.PrestarLibro(prestamo.id, fechaHora_prestamo, fechaHora_vencimiento, prestamo.username_prestatario);

            if (rowsAffected < 1) throw new Exception($"No se pudo prestar el libro con id '{prestamo.id}'.");
            
        }
    }
}
