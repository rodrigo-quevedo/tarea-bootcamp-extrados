using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcular8piezas_biblioteca_de_clases.clases_implementando_interface
{
    //Esta clase la copie y pegue de Reina, es lo mismo pero sin diagonales
    public class Torre : IPieza
    {
        // Contrato con la Interface.
        // La pieza devuelve los siguientes datos:
        public int col { get; private set; }
        public int fila { get; private set; }
        public string simboloPieza { get; private set; }
        public Boolean[,] posicionesAtacadas { get; private set; } = new Boolean[8, 8];



        // Implementacion especifica de cada clase.
        // Se debe setear cada dato segun corresponda:
        public Torre(string simboloPieza)
        {
            this.simboloPieza = simboloPieza;
            inicializarPosicionesAtacadas();
        }

        public void establecerPosicion(int col, int fila)
        {
            this.col = col;
            this.fila = fila;
            calcularPosicionesAtacadas();
        }

        public void inicializarPosicionesAtacadas()
        {
            for (int col = 0; col <= 7; col++)
            {
                for (int fila = 0; fila <= 7; fila++)
                {
                    this.posicionesAtacadas[col, fila] = false;
                }
            }

        }

        void calcularPosicionesAtacadas()
        {
            calcularPosicionesVerticales(this.col, this.posicionesAtacadas);
            calcularPosicionesHorizontales(this.fila, this.posicionesAtacadas);
        }

        void calcularPosicionesVerticales(int col, Boolean[,] posiciones)
        {
            int colTablero = col;

            for (int filaTablero = 0; filaTablero <= 7; filaTablero++)
            {
                posiciones[colTablero, filaTablero] = true;
            }

        }

        void calcularPosicionesHorizontales(int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;

            for (int colTablero = 0; colTablero <= 7; colTablero++)
            {
                posiciones[colTablero, filaTablero] = true;
            }

        }

    }
}
