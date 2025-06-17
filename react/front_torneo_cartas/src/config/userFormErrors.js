import userFormProperties from "./userFormProperties"

const userFormErrors = {
    required: "El campo es obligatorio.",
    tipoString: "El tipo de dato es inválido, debe ser 'string'.",
    tipoNumber: "El tipo de dato es inválido, debe ser 'number'.",

    [userFormProperties.email] : {
        longitud: `Debe tener entre 4 y 70 caracteres.`,
        formato: `Debe ser una dirección email válida.`
    },
    [userFormProperties.nombre_apellido] : {
        longitud: `Debe tener entre 5 y 60 caracteres.`,
        caracteres: `Debe tener solo letras (no se aceptan acentos o tildes) o espacios.`
    },
    [userFormProperties.pais] : {
        paisInvalido: `El pais es invalido.`
    },


    [userFormProperties.alias] : {
        longitud: `Debe tener entre 5 y 25 caracteres.`,
        caracteres: `Debe tener solo letras (no se aceptan acentos o tildes) y numeros.`
    },
    [userFormProperties.foto] : {
        longitud: `Debe tener entre 4 y 200 caracteres.`,
        protocolo: `Debe empezar con 'https://' o 'http://', y terminar con un dominio (ej. '.com')`
    },
   
    [userFormProperties.password] : {
        longitud: `Debe tener entre 4 y 20 caracteres`,
        caracteres: `Debe tener solo letras (no se aceptan acentos o tildes), numeros, o los caracteres: @ # $ %`
    },

    [userFormProperties.confirmPassword]:"Los campos 'Password' y 'Confirmar password' no coinciden.",

    [userFormProperties.rol]:{
        rolInvalido: "El rol es invalido"
    }

}

export default userFormErrors