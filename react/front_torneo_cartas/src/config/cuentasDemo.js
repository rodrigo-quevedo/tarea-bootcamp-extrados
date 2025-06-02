

export const  cuentasDemo = {
    Admin: {
        email: import.meta.env.VITE_DEMO_ADMIN_EMAIL,
        password: import.meta.env.VITE_DEMO_ADMIN_PASSWORD
    },
    Organizador: {
        email: import.meta.env.VITE_DEMO_ORG_EMAIL,
        password: import.meta.env.VITE_DEMO_ORG_PASSWORD
    },
    Juez: {
        email: import.meta.env.VITE_DEMO_JUEZ_EMAIL,
        password: import.meta.env.VITE_DEMO_JUEZ_PASSWORD
    },
    Jugador: {
        email: import.meta.env.VITE_DEMO_JUGADOR_EMAIL,
        password: import.meta.env.VITE_DEMO_JUGADOR_PASSWORD
    }
}