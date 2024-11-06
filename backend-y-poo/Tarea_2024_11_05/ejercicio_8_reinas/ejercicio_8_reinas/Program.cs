// See https://aka.ms/new-console-template for more information
using ejercicio_8_reinas;

Console.WriteLine("Hello, World!");

Tablero miTablero = new Tablero();

miTablero.inicializar();
miTablero.mostrar();

miTablero.calcular8Reinas(0);