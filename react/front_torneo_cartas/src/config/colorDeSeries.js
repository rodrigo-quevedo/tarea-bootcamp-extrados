


export default function colorDeSeries(arraySeries) {//serie de caracteres (ej. ['B', 'G'])
    const colorDeSerie = {
    B: '#FFFFFF', // blanco
    C: '#33FF57', // verde
    D: '#0118D8', // azul
    E: 'cyan', // cyan
    F: '#820007', // bordo
    G: '#bf0029', // rosa
    H: '#FFD433', // amarillo
    I: '#FF8C33', // naranja
    J: '#7cbf00'  // lima
    };

    const fallbackColor = '#CCCCCC'; // fallback gris

    //chequear que es array
    if (!Array.isArray(arraySeries)) {
        console.log("error colores de series, no es un array:", arraySeries)
        return fallbackColor
    };

    if (arraySeries.length === 1) return colorDeSerie[arraySeries[0]] || fallbackColor

    if (arraySeries.length === 2) {
        const c1 = colorDeSerie[arraySeries[0]] || fallbackColor;
        const c2 = colorDeSerie[arraySeries[1]] || fallbackColor;
        return `linear-gradient(to right, ${c1}, ${c2})`;
    }   


}
