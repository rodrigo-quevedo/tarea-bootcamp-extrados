import roles from "../config/roles"
import paisesYZonasHorarias from "../config/paisesYZonasHorarias";

import userFormProperties from "../config/userFormProperties"; //para adaptarse a cambios en la API
import userFormErrors from "../config/userFormErrors";

import {validarString, validarLengthInclusive, validarLetras_Espacio, validarFormatoEmail, validarRequired, validarProtocoloYDominio, validarLetrasNumeros, validarCaracteresPassword} from "./validations";

//Los errores se muestran en orden de importancia de los siguientes aspectos:
//1. required
//2. tipo de dato
//3. longitud de string
//4. caracteres validos

// Se asigna el PRIMER error que salte al state de error,
// de esa manera el usuario puede ir haciendo las correcciones poco a poco,
// siguiendo mensajes de error mas especificos y en orden de importancia.

//Si validacion OK ===> error = null


export function validarNombreApellido(value, setValue, error, setError, required) {
    setValue(value)

    if (required && ! validarRequired(value)) {
        //si no es required, aca no pasa nada
        //si ES required, solo se ejecuta esto cuando NO PASA la validacion 
        //si es required y valido, pasa de largo y no ejecuta esto

         setError({ 
            ...error,
            [userFormProperties.nombre_apellido]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.nombre_apellido]: userFormErrors.tipoString
        })
        return false;    
    }

    if (! validarLengthInclusive(value, 5, 25)) {
        setError({
            ...error,
            [userFormProperties.nombre_apellido]: userFormErrors[userFormProperties.nombre_apellido].longitud
        })
        return false;
    }
        
    if (! validarLetras_Espacio(value)) {
        setError({
            ...error,
            [userFormProperties.nombre_apellido]: userFormErrors[userFormProperties.nombre_apellido].caracteres
        })
        return false;
    }
    
    //OK
    setError({
        ...error,
        [userFormProperties.nombre_apellido]: null
    })
    return true;
}


export function validarEmail(value, setValue, error, setError, required) {
    setValue(value)

    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.email]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.email]: userFormErrors.tipoString
        })
        return false;
    }
        
    if (! validarLengthInclusive(value, 4, 70)) {
        setError({
            ...error,
            [userFormProperties.email]: userFormErrors[userFormProperties.email].longitud
        })
        return false;
    }
    
    if (! validarFormatoEmail(value)){
        setError({
            ...error,
            [userFormProperties.email]: userFormErrors[userFormProperties.email].formato
        })
        return false;
    }
    
    //OK
    setError({
        ...error,
        [userFormProperties.email]: null
    })
    return true;
}


export default function validarPais(value, setValue, error, setError, required) {
    setValue(value)

    if (required && ! validarRequired(value)) {
         setError({ 
            ...error,
            [userFormProperties.pais]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.pais]: userFormErrors.tipoString
        })
        return false;    
    }

    //aca el includes() es CASE SENSITIVE (hace diferencia entre mayusculas y minusculas)
    if (! paisesYZonasHorarias.includes(value)){
        setError({
            ...error,
            [userFormProperties.pais]: userFormErrors[pais].paisInvalido
        })
        return false;
    }

    //OK
    setError({
        ...error,
        [userFormProperties.pais]: null
    })
}

export function validarFotoURL(value, setValue, error, setError, required){
    setValue(value)

    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.foto]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.foto]: userFormErrors.tipoString
        })
        return false;    
    }
    
    if (! validarProtocoloYDominio(value)){
        setError({ 
            ...error,
            [userFormProperties.foto]: userFormErrors[userFormProperties.foto].protocolo
        })
        return false;
    }

    if (! validarLengthInclusive(value, 4, 200)){
        setError({ 
            ...error,
            [userFormProperties.foto]: userFormErrors[userFormProperties.foto].longitud
        })
        return false;
    }
        
    //OK
    setError({ 
        ...error,
        [userFormProperties.foto]: null
    })
    return true;
}


export function validarAlias(value, setValue, error, setError, required){
    setValue(value)

    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.alias]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.alias]: userFormErrors.tipoString
        })
        return false;    
    }

    if (! validarLengthInclusive(value, 5, 25)){
        setError({ 
            ...error,
            [userFormProperties.alias]: userFormErrors[userFormProperties.alias].longitud
        })
        return false;    
    }

    if (! validarLetrasNumeros(value)){
        setError({ 
            ...error,
            [userFormProperties.alias]: userFormErrors[userFormProperties.alias].caracteres
        })
        return false;    
    }

    //OK
    setError({ 
        ...error,
        [userFormProperties.alias]: null
    })
    return true;
}


export function validarPassword(value, setValue, error, setError, required) {
    setValue(value)

    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.password]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.password]: userFormErrors.tipoString
        })
        return false;    
    }

      if (! validarLengthInclusive(value, 4, 20)){
        setError({ 
            ...error,
            [userFormProperties.password]: userFormErrors[userFormProperties.password].longitud
        })
        return false;    
    }

    if (! validarCaracteresPassword(value)){
        setError({ 
            ...error,
            [userFormProperties.password]: userFormErrors[userFormProperties.password].caracteres
        })
        return false;    
    }


    //OK
    setError({ 
        ...error,
        [userFormProperties.password]: null
    })
    return true;
}

//validarPassword() + que sean iguales
//'value' es el campo confirmPassword y 'password' el campo password
export function validarConfirmPassword(password, value, setValue, error, setError, required){
    setValue(value)
    
    //primero que sea valido el campo
    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.confirmPassword]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.confirmPassword]: userFormErrors.tipoString
        })
        return false;    
    }

      if (! validarLengthInclusive(value, 4, 20)){
        setError({ 
            ...error,
            [userFormProperties.confirmPassword]: userFormErrors[userFormProperties.password].longitud
        })
        return false;    
    }

    if (! validarCaracteresPassword(value)){
        setError({ 
            ...error,
            [userFormProperties.confirmPassword]: userFormErrors[userFormProperties.password].caracteres
        })
        return false;    
    }


    //despues, validar que ambas passwords coincidan
    if (password !== value) {
        setError({ 
            ...error,
            [userFormProperties.confirmPassword]: userFormErrors[userFormProperties.confirmPassword]
        })
        return false;
    }

    //OK
    setError({ 
        ...error,
        [userFormProperties.confirmPassword]: null
    })
    return true;
}

export function validarRol(value, setValue, error, setError, required){
    setValue(value)
    
    //primero que sea valido el campo
    if (required && ! validarRequired(value)) {
        setError({ 
            ...error,
            [userFormProperties.rol]: userFormErrors.required
        })
        return false;
    }

    if (! validarString(value)) {
        setError({ 
            ...error,
            [userFormProperties.rol]: userFormErrors.tipoString
        })
        return false;    
    }

     if (! roles.includes(value)){
        setError({
            ...error,
            [userFormProperties.rol]: userFormErrors[rol].rolInvalido
        })
        return false;
    }

    //OK
    setError({ 
        ...error,
        [userFormProperties.rol]: null
    })
    return true;
}