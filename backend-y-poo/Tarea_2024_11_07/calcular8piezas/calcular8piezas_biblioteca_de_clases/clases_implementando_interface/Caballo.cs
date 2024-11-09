using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcular8piezas_biblioteca_de_clases.clases_implementando_interface
{
    public class Caballo : IPieza
    {
        // Contrato con la Interface.
        // La pieza devuelve los siguientes datos:
        public int col { get; private set; }
        public int fila { get; private set; }
        public string simboloPieza { get; private set; }
        public Boolean[,] posicionesAtacadas { get; private set; } = new Boolean[8, 8];



        // Implementacion especifica de cada clase.
        // Se debe setear cada dato segun corresponda:
        public Caballo(string simboloPieza)
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
            int ColTablero = this.col;
            int FilaTablero = this.fila;

            //1 izquierda, 2 arriba
            ColTablero -= 1;
            FilaTablero += 2;

                //->posicion amenazada
            if (ColTablero >= 0 && FilaTablero <= 7 )
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
            if (ColTablero >= 0 && FilaTablero <= 7 )
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
            if (ColTablero <= 7 && FilaTablero <= 7 )
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
            if (ColTablero >= 0 && FilaTablero >= 0 )
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
            if (ColTablero >= 0 && FilaTablero >= 0 )
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
            if (ColTablero <= 7 && FilaTablero >= 0 )
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





    }
}
