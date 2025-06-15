import { createTheme, ThemeProvider as MUIThemeProvider, CssBaseline } from '@mui/material';

import { useTema } from '../hooks/useCambiarTheme.jsx';
import { lightPalette, darkPalette } from './palettes';

export default function ThemeProvider({ children }) {
  const {tema} = useTema();

  console.log("tema en themeprovider:", tema)

  const theme = createTheme({
    palette: tema === 'light' ? lightPalette : darkPalette
  });

  return (
    <MUIThemeProvider theme={theme}>
        <CssBaseline/>
        {children}
    </MUIThemeProvider>
  );
}
