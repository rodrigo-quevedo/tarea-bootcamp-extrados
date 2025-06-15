import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './styles/index.css'
import { BrowserRouter } from "react-router";
import App from './App.jsx'
import { ThemeProviderContext } from './hooks/useCambiarTheme.jsx';
import ThemeProvider from './theme/ThemeProvider.jsx';

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ThemeProviderContext>
        <ThemeProvider>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </ThemeProvider>
    </ThemeProviderContext>
  </StrictMode>,
)
