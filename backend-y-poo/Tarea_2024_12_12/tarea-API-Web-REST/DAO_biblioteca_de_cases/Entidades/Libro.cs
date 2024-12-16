using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO_biblioteca_de_cases.Entidades
{
    public class Libro
    {
        public int id {  get; set; }
        public string titulo { get; set; }
        public DateTime? fechaHora_prestamo { get; set; }//puede ser null
        public DateTime? fechaHora_vencimiento { get; set; }//puede ser null
        public string? username_prestatario { get; set; }//puede ser null

        public Libro(int id, string titulo, DateTime fechaHora_prestamo, DateTime fechaHora_vencimiento, string username_prestatario)
        {
            this.id = id;
            this.titulo = titulo;
            this.fechaHora_prestamo = fechaHora_prestamo;
            this.fechaHora_vencimiento = fechaHora_vencimiento;
            this.username_prestatario = username_prestatario;
        }

        public void mostrarDatos()
        {
            Console.WriteLine($"ID: {this.id}");
            Console.WriteLine($"Titulo: {this.titulo}");
            Console.WriteLine($"Fecha hora prestamo: {this.fechaHora_prestamo}");
            Console.WriteLine($"Fecha hora vencimiento: {this.fechaHora_vencimiento}");
            Console.WriteLine($"Username prestatario: {this.username_prestatario}");
        }
    }
}
