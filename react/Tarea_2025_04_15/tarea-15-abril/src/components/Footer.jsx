

export default function Footer(){
    return (
        <footer>

            <div className="footer-container">
                <div className="footer-contacto">
                    <h3>Contacto</h3>
                    <p>tiendaropa@tiendaropa.com</p>
                    <p>+ 00 123 456 7894</p>
                </div>

                <nav className="footer-nav">
                    <ul>
                        <li><a href="#">Sobre nosotros</a></li>
                        <li><a href="#">FAQ</a></li>
                        <li><a href="#">Política de Envíos</a></li>
                        <li><a href="#">Política de Devoluciones</a></li>
                    </ul>
                </nav>
            </div>

            <p className="copyright">© 2025 Tienda Ropa. Todos los derechos reservados.</p>
        
        </footer>
    )
}