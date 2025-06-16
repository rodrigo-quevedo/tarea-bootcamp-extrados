export function validarRequired(value) {
    if (value === null || value === undefined || value === "") return false;

    return true;
}

// minimo y maximo INCLUSIVE
export function validarLengthInclusive(value, min, max){
    if (value.length >= min && value.length <= max) return true

    return false
}

// minimo y maximo sin incluir
export function validarLengthSinIncluir(value, min, max){
    if (value.length > min && value.length < max) return true

    return false
}

export function validarString(value){
    if (typeof value === 'string') return true

    return false
}

export function validarNumber(value){
    if (typeof value === 'number') return true

    return false
}

export function validarLetras_Espacio(value){
    if (/^[a-zA-Z ]*$/.test(value)) return true
    
    return false
}

export function validarLetrasNumeros(value){
    if (/^[a-zA-Z0-9]*$/.test(value)) return true

    return false
}

export function validarCaracteresPassword(value){
    if (/^[a-zA-Z0-9@#$%]*$/.test(value)) return true

    return false
}

//solo valida http/s y que haya al menos un formato dominio (pero no valida TLD reales)
export function validarProtocoloYDominio(value){
    if (/^https?:\/\/.+[.].+/i.test(value)) return true;

    return false;
}

//validacion bastante simple y permisiva: 
// algo antes del @, un @, algo despues del @, un ., y al ultimo un dominio con minimo 2 caracteres
export function validarFormatoEmail(value){
    if(/^[\w]+@[\w.]+\.\w{2,}$/.test(value)) return true;

    return false
}