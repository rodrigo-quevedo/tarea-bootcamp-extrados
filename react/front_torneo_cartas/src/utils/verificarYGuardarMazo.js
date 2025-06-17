import coleccionarCartas from "../services/coleccionarCartas";


export default function verificarYGuardarMazo(arrayIDcartasSeleccionadas, setPopupError, setPopupExito) {
    // console.log("array ID cartas seleccionadas", arrayIDcartasSeleccionadas)
    // arrayIDcartasSeleccionadas.forEach(element => {
    //     console.log(typeof element)
    // });

    
    if (arrayIDcartasSeleccionadas.length < 8 || arrayIDcartasSeleccionadas.length > 15) {
        setPopupError(`Debe seleccionar entre 8 y 15 cartas. Seleccionó ${arrayIDcartasSeleccionadas.length}.`);
        return;
    }
    
    const idsUnicos = new Set(arrayIDcartasSeleccionadas);
    console.log("idsUnicos:", idsUnicos)
    if (arrayIDcartasSeleccionadas.length !== idsUnicos.size) {
        setPopupError("Hay cartas duplicadas.");
        return;
    }


    // Si pasa todas las validaciones
    coleccionarCartas(arrayIDcartasSeleccionadas, setPopupError, setPopupExito);
};