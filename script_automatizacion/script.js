console.log('ok');

//Campos de las tablas a crear:
const CAMPOS_TABLA_DEPARTAMENTOS = ['ID_DEPARTAMENTO', 'NOMBRE', 'ID_MANAGER', 'ID_LOCACION'];
const CAMPOS_TABLA_EMPLEADOS = ['ID_EMPLEADO', 'NOMBRE', 'APELLIDO', 'EMAIL', 'TELEFONO', 'FECHA_EN', 'ID_TRABAJO', 'SALARIO', 'COMISION', 'ID_MANAGER', 'ID_DEPARTAMENTO'];
const CAMPOS_TABLA_LOCACIONES = ['ID_LOCACION', 'DIRECCION', 'CODIGO_POSTAL', 'CIUDAD', 'ESTADO_PROVINCIA', 'ID_PAIS'];

//DB
const mysql = require('mysql2/promise');
let connection;
const conectarDB = async () =>{
    try {
        connection = await mysql.createConnection({
            host: 'localhost',
            user: 'root',
            password: process.env.MYSQL_PASSWORD,
            database: 'tarea',
            multipleStatements: true
        }); 
        console.log(`Conexión: ${connection}`)
        return connection;

    }
    catch(err){
        console.log(err)
    }
}

const dropTablas = async () => {
    try {
        const [results, fields] = await connection.query(
            `
            DROP TABLE departamentos;
            DROP TABLE empleados;
            DROP TABLE locaciones;
            `
            );
        console.log(`Result from query:`,results); // results contains rows returned by server

        return results
    }
    catch(err) {
        console.log(err)
    }
    
}

const crearTablas = async () => {
    try {
        const [results, fields] = await connection.query(            
           `
            CREATE TABLE locaciones (
                ID_LOCACION INTEGER PRIMARY KEY,
                DIRECCION VARCHAR(60) NOT NULL,
                CODIGO_POSTAL VARCHAR(15) NOT NULL,
                CIUDAD VARCHAR (25) NOT NULL,
                ESTADO_PROVINCIA VARCHAR(25),
                ID_PAIS VARCHAR(2) NOT NULL
            );

            CREATE TABLE empleados (
                ID_EMPLEADO INTEGER PRIMARY KEY,
                NOMBRE VARCHAR(25) NOT NULL,
                APELLIDO VARCHAR(20) NOT NULL,
                EMAIL VARCHAR(30) NOT NULL,
                TELEFONO VARCHAR(25) NOT NULL,
                FECHA_EN VARCHAR(10) NOT NULL,
                ID_TRABAJO VARCHAR(10) NOT NULL,
                SALARIO FLOAT(9,2) NOT NULL,
                COMISION FLOAT(9,2) NOT NULL,
                ID_MANAGER INTEGER,
                ID_DEPARTAMENTO INTEGER
            );

            CREATE TABLE departamentos (
                ID_DEPARTAMENTO INTEGER NOT NULL PRIMARY KEY,
                NOMBRE VARCHAR(30) NOT NULL,
                ID_MANAGER INTEGER,
                ID_LOCACION INTEGER NOT NULL,
                FOREIGN KEY(ID_MANAGER) REFERENCES empleados(ID_EMPLEADO),
                FOREIGN KEY (ID_LOCACION) REFERENCES locaciones(ID_LOCACION)
            );
            `
        );
        console.log(`Result from query:`,results); // results contains rows returned by server

        return results
    }
    catch(err) {
        console.log(err)
    }
}

const fs = require('node:fs')
const extraerDatosDeArchivo = (rutaArchivo) => {
    //Obtener archivo:
    let archivo = fs.readFileSync(rutaArchivo);
    archivoString = archivo.toString();

    //Parsear datos:
    let lineas = archivoString.split('\r\n');
    //le borro las primeras 3 líneas y la última
    lineas.splice(0,3);
    lineas.pop();

    for (let iLinea in lineas){ 
        lineas[iLinea] = lineas[iLinea].split('|')
        //El split hace que sobre un elemento al principio así que lo elimino
        lineas[iLinea].shift()
        //También sobra uno al final, lo elimino
        lineas[iLinea].pop()

        //Quitar el whitespace al dato
        for (let iArr in lineas[iLinea]) {
            lineas[iLinea][iArr] = lineas[iLinea][iArr].trim();
        }
    }
    console.log(lineas)
    return lineas;
}

const cargarDatosDeArchivoEnTabla = async (rutaArchivo, nombreTabla, arrCamposDeTabla) => {
    let arraysDeDatos = extraerDatosDeArchivo(rutaArchivo);
    let datosTablaString = arrCamposDeTabla.toString();
    

    let queryString = 
        `
        INSERT INTO ${nombreTabla}(${datosTablaString})
        VALUES 
        `

    for (let indexArrDatos in arraysDeDatos) {
        queryString += (indexArrDatos == 0) ? 
        `
        (${arraysDeDatos[indexArrDatos].toString()})
        `
        :
        `
        ,(${arraysDeDatos[indexArrDatos].toString()})
        `
    }

    try {
        const [results, fields] = await connection.query(queryString);
        console.log(`Result from query:`,results); // results contains rows returned by server

        return results
    }
    catch(err) {
        console.log(err)
    }



}

const ejecutarTodo = async () => {
    await conectarDB();
    await dropTablas();
    await crearTablas();
    await cargarDatosDeArchivoEnTabla('./tablas/locaciones.txt', 'locaciones', CAMPOS_TABLA_LOCACIONES)
    // await cargarDatosDeArchivoEnTabla('./tablas/empleados.txt', 'empleados', CAMPOS_TABLA_EMPLEADOS)
    // await cargarDatosDeArchivoEnTabla('./tablas/departamentos.txt', 'departamentos', CAMPOS_TABLA_DEPARTAMENTOS)

    console.log('Completado.')
    process.exit()
}

ejecutarTodo();


