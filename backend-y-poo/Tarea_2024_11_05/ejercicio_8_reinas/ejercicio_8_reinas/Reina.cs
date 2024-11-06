using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ejercicio_8_reinas
{
    public class Reina
    {
        public int col { get; private set; }
        public int fila { get; private set; }


        public Reina(int col, int fila)
        {
            this.col = col;
            this.fila = fila;
        }

        public Boolean[,] calcularPosicionesVerticalesNoDisponibles(int col)
        {
            var posicionesVerticalesNoDisponibles = new Boolean[8, 8];

            for (int filaTablero = 0; filaTablero <= 7; filaTablero++)
            {
                posicionesVerticalesNoDisponibles[col, filaTablero] = true;
            }

            return posicionesVerticalesNoDisponibles;
        }

        public Boolean[,] calcularPosicionesHorizontalesNoDisponibles(int fila)
        {
            var posicionesVerticalesNoDisponibles = new Boolean[8, 8];

            for (int colTablero = 0; colTablero <= 7; colTablero++)
            {
                posicionesVerticalesNoDisponibles[colTablero, fila] = true;
            }

            return posicionesVerticalesNoDisponibles;
        }

        public Boolean[,] calcularPosicionesDiagonalesNoDisponibles(int col, int fila)
        {
            var posicionesDiagonalesNoDisponibles = new Boolean[8, 8];

      
            calcularDiagonalArribaIzquierda(col, fila, posicionesDiagonalesNoDisponibles);
            calcularDiagonalArribaDerecha(col, fila, posicionesDiagonalesNoDisponibles);



            return posicionesDiagonalesNoDisponibles;
        }

        public void calcularDiagonalArribaIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;

            for (int colTablero = col; colTablero >= 0; colTablero--)
            {
                posiciones[colTablero, filaTablero] = true;

                filaTablero++;
            }

            return;
        }

        public void calcularDiagonalArribaDerecha(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;

            for (int colTablero = col; colTablero <= 7; colTablero++)
            {
                posiciones[colTablero, filaTablero] = true;

                filaTablero++;
            }

            return;
        }

        public void calcularDiagonalAbajoIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;

            for (int colTablero = col; colTablero >= 0 ; colTablero--)
            {
                posiciones[colTablero, filaTablero] = true;

                filaTablero--;
            }

            return;
        }


    }
}
