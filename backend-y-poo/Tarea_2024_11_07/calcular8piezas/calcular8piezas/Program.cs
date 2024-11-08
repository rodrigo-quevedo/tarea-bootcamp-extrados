// See https://aka.ms/new-console-template for more information
using calcular8piezas_biblioteca_de_clases;
using calcular8piezas_biblioteca_de_clases.clases_implementando_interface;

Console.WriteLine("Hello, World!");

Tablero tablero = new Tablero();

tablero.calcular8Piezas(0, new Reina(), 0);
tablero.calcular8Piezas(0, new Torre(), 0);