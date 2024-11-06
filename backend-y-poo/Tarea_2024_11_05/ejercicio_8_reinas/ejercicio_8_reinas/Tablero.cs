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
            Console.WriteLine();
        }


        //lo saco de la función por si tengo que hacer recursión, evitando crear varias veces este objeto
        Random random = new Random();
        public void calcular8Reinas(int cantidadReinas)
        {
            Console.WriteLine("Entrando a calcular8Reinas()...");
            //checkear reseteo del tablero
            Boolean resetearlo = true;

            for (int col = 0;col <= 7; col++)
            {
                for(int fila= 0; fila <= 7; fila++)
                {
                    if (this.posicionesDisponibles[col, fila]) 
                    {
                        resetearlo = false;
                    }
                }
            }

            if (resetearlo) 
            {
                this.inicializar();
                cantidadReinas = 0;
                this.mostrar();
            }

            //meter reina en el tablero
            int randCol = random.Next(0, 8);
            int randFila = random.Next(0, 8);

            //ya se que hay posiciones disponibles, y voy a elegir una al azar
            Boolean reinaAgregada = false;

            Console.WriteLine("Agregando otra reina...");
            while (reinaAgregada == false) 
            { 
                if (posicionesDisponibles[randCol, randFila] == true)
                {
                    new Reina(randCol, randFila, this.posicionesDisponibles, this.posicionesReina);
                    reinaAgregada = true;
                    cantidadReinas++;
                    this.mostrar();
                }
                else
                {
                    randCol = random.Next(0, 8);
                    randFila = random.Next(0, 8);
                }
            }
            Console.WriteLine("Reina agregada...");
            //checkeo si ya estan las 8 reinas, sino, vuelvo a ejecutar esta funcion
            Console.WriteLine("Chequeando cantidad de reinas...");
            if (cantidadReinas == 8) 
            {
                Console.WriteLine("Ejercicio resuelto:");
                this.mostrar();
                Console.WriteLine($"Cantidad de reinas: {cantidadReinas}");
                return;
            }
            else
            {
                Console.WriteLine($"Cantidad de reinas no es suficiente:{cantidadReinas}");
                calcular8Reinas(cantidadReinas);
            }

            return;
        }

    }
}
