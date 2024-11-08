using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcular8piezas_biblioteca_de_clases
{
    public interface IPieza
    {
        public int col { get; }
        public int fila { get;}
        public string simboloPieza { get;}
        public Boolean [,] posicionesAtacadas { get;}

        public void establecerPosicion(int col, int fila);
        public void inicializarPosicionesAtacadas();
    }
}
