// See https://aka.ms/new-console-template for more information
using calcular8piezas_biblioteca_de_clases;
using calcular8piezas_biblioteca_de_clases.clases_implementando_interface;


Tablero tablero = new Tablero();

//Se recomienda ejecutar 1 pieza a la vez para poder ver el desarrollo de la solucion.

// Piezas existentes:

tablero.calcular8Piezas(0, new Reina("R"), 0);

tablero.calcular8Piezas(0, new Torre("T"), 0);

tablero.calcular8Piezas(0, new Caballo("C"), 0);


// ReinaACaballo: Reina+Caballo, pieza inexistente.
// Simbolo: &
// Esta pieza no tiene solucion:
//|  -  -  -  -  X  X  X  X  |
//|  -  -  -  -  X  X  X  X  |
//|  X  X  X  X  X  X  &  X  |
//|  -  -  -  -  X  X  X  X  |
//|  -  -  -  -  X  X  X  X  |
//|  -  -  -  X  -  -  X  -  |
//|  -  -  X  -  -  -  X  -  |
//|  -  X  -  -  -  -  X  -  |
tablero.calcular8Piezas(0, new ReinaACaballo("&"), 0);