namespace tarea_API_Web_REST.Utils.RequestBodyParams
{
    public class PrestamoLibro
    {
        public int id { get; set; }
        public string username_prestatario { get; set; }
        public string fechaHora_prestamo { get; set; }

        public void mostrarDatos()
        {
            Console.WriteLine($"Id libro: {this.id}");
            Console.WriteLine($"Username: {this.username_prestatario}");
            Console.WriteLine($"Fecha hora prestamo: {this.fechaHora_prestamo}");
        }
    }
}
