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


        public Reina(int col, int fila, Boolean[,] posicionesDisponibles, Boolean[,] posicionesReina)
        {
            this.col = col;
            this.fila = fila;

            posicionesReina[col, fila] = true;

            //Cuando la reina empieza a existir, automaticamente inhabilita las posiciones correspondientes
            this.calcularPosicionesVerticalesNoDisponibles(this, posicionesDisponibles);
            this.calcularPosicionesHorizontalesNoDisponibles(this, posicionesDisponibles);
            this.calcularPosicionesDiagonalesNoDisponibles(this, posicionesDisponibles);
        }

        public Boolean[,] calcularPosicionesVerticalesNoDisponibles(Reina reina, Boolean[,] posicionesDisponibles)
        {
            int col = reina.col;

            for (int filaTablero = 0; filaTablero <= 7; filaTablero++)
            {
                posicionesDisponibles[col, filaTablero] = false;
            }

            return posicionesDisponibles;
        }

        public Boolean[,] calcularPosicionesHorizontalesNoDisponibles(Reina reina, Boolean[,] posicionesDisponibles)
        {
            int fila = reina.fila;

            for (int colTablero = 0; colTablero <= 7; colTablero++)
            {
                posicionesDisponibles[colTablero, fila] = false;
            }

            return posicionesDisponibles;
        }

        public Boolean[,] calcularPosicionesDiagonalesNoDisponibles(Reina reina, Boolean[,] posicionesDisponibles)
        {
            int col = reina.col;
            int fila = reina.fila;

            calcularDiagonalArribaIzquierda(col, fila, posicionesDisponibles);
            calcularDiagonalArribaDerecha(col, fila, posicionesDisponibles);
            calcularDiagonalAbajoIzquierda(col, fila, posicionesDisponibles);
            calcularDiagonalAbajoDerecha(col, fila, posicionesDisponibles);

            return posicionesDisponibles;
        }

        public void calcularDiagonalArribaIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero > -1 && filaTablero < 8)
            {
                posiciones[colTablero, filaTablero] = false;

                colTablero--;
                filaTablero++;
            }

            return;
        }

        public void calcularDiagonalArribaDerecha(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero < 8 && filaTablero < 8)
            {
                posiciones[colTablero, filaTablero] = false;

                colTablero++;
                filaTablero++;
            }

            return;
        }

        public void calcularDiagonalAbajoIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero > -1 && filaTablero > -1)
            {
                posiciones[colTablero, filaTablero] = false;

                colTablero--;
                filaTablero--;
            }

            return;
        }

        public void calcularDiagonalAbajoDerecha(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero < 8 && filaTablero > -1)
            {
                posiciones[colTablero, filaTablero] = false;

                colTablero++;
                filaTablero--;
            }

            return;
        }


    }
}
