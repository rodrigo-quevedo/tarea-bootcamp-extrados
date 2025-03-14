# Consigna del Trabajo Final Backend - Curso Extrados 2024-2025

## Introducción

El trabajo final consiste en la programación de un backend para un sistema de administración de torneos de eliminaciones online de juegos de cartas coleccionables.

## Cartas

Las cartas se publican en “series”. una carta puede pertenecer a una o múltiples series (carta X pertenece a la serie 1, carta Y pertenece tanto a la serie 1 como 2).

## Mazos

Un mazo de cartas cuenta con 15 cartas únicas (no puede haber repetidas).

## Juegos/Partidas

Un juego debe durar un máximo de 30 minutos, consta de 1 jugador vs 1 jugador, y siempre hay un ganador (no se permite empates).

## Torneo

Un torneo es organizado por un “Organizador”, este asigna “Jueces” para oficializar los resultados del torneo, y el sistema debe planificar, en base a la cantidad de jugadores y el tiempo disponible, en que días y horarios se desarrollaran los distintos juegos del torneo.

Cada torneo puede limitar las series de cartas con las que se puede jugar durante el mismo.

**El torneo tiene 3 fases básicas:**

<ins>Fase 1 “registro”</ins>

Durante la fase de registro, los jugadores deben poder registrarse, junto al mazo de cartas con el que entraran al torneo (el mismo armado únicamente con las cartas pertenecientes al jugador y a las series que acepten en el torneo). Para simplificar el trabajo, haremos que el registro lo termine el organizador manualmente.

<ins>Fase 2 “torneo”</ins>

Durante la fase de torneo, el sistema debe evitar que más jugadores se inscriban, y el sistema debe planificar los días y horarios de los juegos que serán necesarios para llevar a cabo el torneo, además de que jugadores jugaran cada juego durante la 1ra ronda.

La fase 2 del torneo va a tener N cantidad de mini-fases, o rondas de juegos, donde se eliminan los perdedores de la anterior ronda de juego y se re-planifican los juegos con los ganadores para otra ronda de juegos. Una vez que todos los juegos de una ronda de juegos terminen, se debe pasar automáticamente a la siguiente ronda de juegos, junto a la planificación.

<ins>Fase 3 “finalización”</ins>

Una vez que la final se juegue, y este juego se oficialice, debe quedar el torneo como “finalizado”.


## Roles

El sistema debe contar con los siguientes roles para los usuarios:

### Administrador
- El rol de administrador debe poder crear, editar y eliminar otros administradores, organizadores, jueces y jugadores.
- El administrador debe poder ver y cancelar torneos.
- El sistema debe tener al menos un administrador en la base de datos, y no se pueden registrar administradores por su cuenta.

### Organizador
- El rol de organizador debe poder crear, editar y cancelar torneos.
- El organizador también debe poder hacer avanzar un torneo a la siguiente fase.
- El organizador también debe poder modificar los juegos del torneo.
- Debe ser creado por un administrador.
- Debe poder crear un Juez.

### Juez
- Un juez debe poder oficializar el resultado de un juego dentro de un evento, y descalificar un jugador de un evento de ser necesario.
- Debe ser creado por un Organizador.

### Jugador
- Un jugador debe poder registrar las cartas que tiene en su colección.
- Un jugador también debe poder registrarse en un torneo, y armar un mazo de cartas con las cartas que cuenta para entrar a cada torneo.
- El jugador se puede registrar en el sistema por su cuenta.

## Información básica a guardar

La información básica a guardar es lo que se espera que el sistema pueda devolver al frontend. No significa que cada uno de estos datos deba ser un campo en la base de datos, el sistema también puede calcularlos en base a otros datos.
Tampoco es la única información que se debe guardar, de ser necesario se puede guardar más información.

### Jugador

- Nombre y apellido
- Alias
- País
- Email
- Juegos ganados
- Juegos perdidos
- Torneos ganados
- Descalificaciones y la razón por la que fue descalificado
- Foto/avatar
- Lista de cartas que posee.

### Juez
- Nombre y apellido
- Alias
- Email
- País
- Torneos que oficializo
- Foto/avatar

### Organizador
- Nombre y apellido
- Email
- País
- Torneos que organizó

### Administrador

- Usuario

### Torneo

- Fecha y hora de inicio
- Fecha y hora de fin
- País
- Lista de jueces
- Lista de series de cartas habilitadas.
- Fase
- Jugadores inscriptos
- Maso de cartas con el que entro cada jugador
- Resultados de cada juego
- Ganador

### Juegos
- Fecha y hora de inicio
- Fecha y hora de fin
- Jugadores que participan en el mismo
- Torneo del que es parte
- Ganador

### Cartas

- Ilustración
- Ataque
- Defensa
- Series a la que pertenece

### Series

- Nombre
- Lista de cartas a las que pertenece esta serie
- Fecha de salida de la serie.


## Notas finales:

- El admin debe poder ver quién creó a qué organizador/juez.
- Un juez solo debe poder oficializar un juego que pertenezca a un torneo en el que esté autorizado, y solo luego de que este juego haya empezado.
- Los jugadores solo podrán ver el “alias” de otros jugadores y de los jueces. Solo los organizadores y administradores deben poder ver su nombre y apellido.
- El alias de cada jugador y juez debe ser único.


