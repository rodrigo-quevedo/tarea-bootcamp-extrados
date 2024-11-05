using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejercicio_8_reinas
{
    public class Tablero
    {

        public Boolean[,] posicionesDisponibles { get; set; } = new Boolean[8, 8];
        public Boolean[,] posicionesReina { get; set; } = new Boolean[8, 8];

        public void inicializar()
        {
            Console.WriteLine("dentro de inicializar");

            for (int col = 0; col <= 7; col++)
            {
                for (int fila = 0; fila <= 7; fila++) 
                {
                    this.posicionesDisponibles[col,fila] = true;

                    this.posicionesReina[col, fila] = false;
                }
            }
            this.posicionesReina[0, 0] = true;
        }

        public void mostrar()
        {
            for (int col = 0; col <= 7; col++)
            {
                Console.Write(" | ");
            
                for (int fila = 0; fila <= 7; fila++)
                {
                    if (this.posicionesReina[col, fila])
                    {
                        Console.Write(" R ");
                    }
                    else
                    {
                        Console.Write(
                            $"{(this.posicionesDisponibles[col, fila] ? " - " : " X ")}"
                        );
                    }
                }

                Console.Write(" | \n");
            }
        }


    }
}
