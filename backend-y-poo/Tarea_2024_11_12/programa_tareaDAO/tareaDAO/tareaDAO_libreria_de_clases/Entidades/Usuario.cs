namespace tareaDAO_libreria_de_clases.Entidades
{
    public class Usuario
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Fecha_baja { get; set; }

        public void mostrarDatos()
        {
            Console.WriteLine($"Id: {this.id} " +
                $"| Nombre: {this.Nombre} " +
                $"| Edad: {this.Edad} " +
                $"| Activo : {(this.Fecha_baja ==null ? "si":"no")}");
        }
    }
}
