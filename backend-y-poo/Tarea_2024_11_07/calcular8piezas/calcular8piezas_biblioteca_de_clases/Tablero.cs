namespace calcular8piezas_biblioteca_de_clases
{
    public class Tablero
    {
        public Boolean[,] posicionesDisponibles { get; set; } = new Boolean[8, 8];
        public Boolean[,] posicionesPieza { get; set; } = new Boolean[8, 8];

        public void inicializar()
        {
            Console.WriteLine("dentro de inicializar");

            for (int col = 0; col <= 7; col++)
            {
                for (int fila = 0; fila <= 7; fila++)
                {
                    this.posicionesDisponibles[col, fila] = true;

                    this.posicionesPieza[col, fila] = false;
                }
            }
        }

        public void mostrar(string simboloPieza)
        {
            for (int col = 0; col <= 7; col++)
            {
                Console.Write(" | ");

                for (int fila = 0; fila <= 7; fila++)
                {
                    if (this.posicionesPieza[col, fila])
                    {
                        Console.Write(simboloPieza);
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
        public void calcular8Piezas(int cantidadPiezas, IPieza pieza, int cantidadIntentos)
        {
            Console.WriteLine("Entrando a calcular8Piezas()...");
            //checkear reseteo del tablero
            Boolean resetearlo = true;

            for (int col = 0; col <= 7; col++)
            {
                for (int fila = 0; fila <= 7; fila++)
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
                cantidadPiezas = 0;
                cantidadIntentos++;
                this.mostrar(pieza.simboloPieza);
            }

            // Chequear cantidad de intentos (llegada cierta cantidad de intentos, la pieza no tiene solucion):
            if (cantidadIntentos > 100)
            {
                Console.WriteLine("El programa no puede encontrar una solucion para esta pieza.");
            }

            //meter pieza en el tablero
            int randCol = random.Next(0, 8);
            int randFila = random.Next(0, 8);

            //ya se que hay posiciones disponibles, y voy a elegir una al azar
            Boolean piezaAgregada = false;

            Console.WriteLine("Agregando otra pieza...");
            while (piezaAgregada == false)
            {
                if (posicionesDisponibles[randCol, randFila] == true)
                {
                    //esto lo tengo que cambiar:
                    //IPieza pieza = new IPieza(randCol, randFila, this.posicionesDisponibles, this.posicionesReina);


                    piezaAgregada = true;
                    cantidadPiezas++;
                    this.mostrar(pieza.simboloPieza);
                }
                else
                {
                    randCol = random.Next(0, 8);
                    randFila = random.Next(0, 8);
                }
            }
            Console.WriteLine("Pieza agregada...");
            //checkeo si ya estan las 8 reinas, sino, vuelvo a ejecutar esta funcion
            Console.WriteLine("Chequeando cantidad de piezas...");
            if (cantidadPiezas == 8)
            {
                Console.WriteLine("Ejercicio resuelto:");
                this.mostrar(pieza.simboloPieza);
                Console.WriteLine($"Cantidad de piezas: {cantidadPiezas}");
                return;
            }
            else
            {
                Console.WriteLine($"Cantidad de piezas no es suficiente:{cantidadPiezas}");
                calcular8Piezas(cantidadPiezas, pieza, cantidadIntentos);
            }

            return;
        }

        public void actualizarTablero()
        {

        }


    }
}
