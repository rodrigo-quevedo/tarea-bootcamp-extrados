//no se si va en src/hooks/ o src/context/

import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

export function ThemeProviderContext ({ children }) {
    const [tema, setTema] = useState(() => {
        const temaGuardado = localStorage.getItem('theme');
        
        if (temaGuardado == null) return 'light';

        else if (temaGuardado === 'light' || temaGuardado === 'dark') return temaGuardado;

        else {
            console.log("hook useCambiarTheme: Error al setear el theme");
            return undefined
        }
    });

  const cambiarTema = () => {
    const nuevoTema = tema === 'light' ? 'dark' : 'light';
    setTema(nuevoTema);
    localStorage.setItem('theme', nuevoTema);
  };

  return (
    <ThemeContext.Provider value={{ tema, cambiarTema }}>
      {children}
    </ThemeContext.Provider>
  );
};

export const useTema = () => useContext(ThemeContext);
