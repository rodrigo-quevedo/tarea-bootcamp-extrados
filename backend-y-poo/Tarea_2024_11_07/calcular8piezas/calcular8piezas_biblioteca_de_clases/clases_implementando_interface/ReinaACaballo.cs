using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcular8piezas_biblioteca_de_clases.clases_implementando_interface
{
    public class ReinaACaballo : IPieza
    {
        // Contrato con la Interface.
        // La pieza devuelve los siguientes datos:
        public int col { get; private set; }
        public int fila { get; private set; }
        public string simboloPieza { get; private set; }
        public Boolean[,] posicionesAtacadas { get; private set; } = new Boolean[8, 8];



        // Implementacion especifica de cada clase.
        // Se debe setear cada dato segun corresponda:
        public ReinaACaballo(string simboloPieza)
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
            calcularPosicionesCaballo();

            calcularPosicionesReina();

        }

        void calcularPosicionesCaballo ()
        {
            int ColTablero = this.col;
            int FilaTablero = this.fila;

            //1 izquierda, 2 arriba
            ColTablero -= 1;
            FilaTablero += 2;

            //->posicion amenazada
            if (ColTablero >= 0 && FilaTablero <= 7)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //2 izquierda, 1 arriba
            ColTablero -= 2;
            FilaTablero += 1;

            //->posicion amenazada
            if (ColTablero >= 0 && FilaTablero <= 7)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //1 derecha, 2 arriba
            ColTablero += 1;
            FilaTablero += 2;

            //->posicion amenazada
            if (ColTablero <= 7 && FilaTablero <= 7)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //2 derecha, 1 arriba
            ColTablero += 2;
            FilaTablero += 1;

            //->posicion amenazada
            if (ColTablero <= 7 && FilaTablero <= 7)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //1 izquierda, 2 abajo
            ColTablero -= 1;
            FilaTablero -= 2;

            //->posicion amenazada
            if (ColTablero >= 0 && FilaTablero >= 0)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //2 izquierda, 1 abajo
            ColTablero -= 2;
            FilaTablero -= 1;

            //->posicion amenazada
            if (ColTablero >= 0 && FilaTablero >= 0)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //2 derecha, 1 abajo
            ColTablero += 2;
            FilaTablero -= 1;

            //->posicion amenazada
            if (ColTablero <= 7 && FilaTablero >= 0)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;



            //1 derecha, 2 abajo
            ColTablero += 1;
            FilaTablero -= 2;

            //->posicion amenazada
            if (ColTablero <= 7 && FilaTablero >= 0)
            {
                this.posicionesAtacadas[ColTablero, FilaTablero] = true;
            }
            //->reset de las variables para poder seguir usandolas
            ColTablero = this.col;
            FilaTablero = this.fila;
        }

        void calcularPosicionesReina()
        {
            calcularPosicionesVerticales(this.col, this.posicionesAtacadas);
            calcularPosicionesHorizontales(this.fila, this.posicionesAtacadas);

            calcularDiagonalArribaIzquierda(this.col, this.fila, this.posicionesAtacadas);
            calcularDiagonalArribaDerecha(this.col, this.fila, this.posicionesAtacadas);
            calcularDiagonalAbajoIzquierda(this.col, this.fila, this.posicionesAtacadas);
            calcularDiagonalAbajoDerecha(this.col, this.fila, this.posicionesAtacadas);

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

        void calcularDiagonalArribaIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero > -1 && filaTablero < 8)
            {
                posiciones[colTablero, filaTablero] = true;

                colTablero--;
                filaTablero++;
            }

            return;
        }

        void calcularDiagonalArribaDerecha(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero < 8 && filaTablero < 8)
            {
                posiciones[colTablero, filaTablero] = true;

                colTablero++;
                filaTablero++;
            }

            return;
        }

        void calcularDiagonalAbajoIzquierda(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero > -1 && filaTablero > -1)
            {
                posiciones[colTablero, filaTablero] = true;

                colTablero--;
                filaTablero--;
            }

            return;
        }

        void calcularDiagonalAbajoDerecha(int col, int fila, Boolean[,] posiciones)
        {
            int filaTablero = fila;
            int colTablero = col;

            while (colTablero < 8 && filaTablero > -1)
            {
                posiciones[colTablero, filaTablero] = true;

                colTablero++;
                filaTablero--;
            }

            return;
        }

    }
}
