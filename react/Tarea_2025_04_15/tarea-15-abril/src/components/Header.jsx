import logo from "../assets/logo.png"

export default function Header(){
    console.log(logo)
    return (
        <header className="header">
            <a href="#">
                <img src={logo}
                    alt="Tienda Ropa logo"
                    className="website-logo"/>
            </a>


            <nav className="nav__categorias">
                <ul className="nav__categorias--lista">
                    <li><a href="#">Hombres</a></li>
                    <li><a href="#">Mujeres</a></li>
                    <li><a href="#">Niños</a></li>
                    <li><a href="#">Ofertas</a></li>
                    <li><a href="#">Sucursales</a></li>
                </ul>
            </nav>


            <div className="acciones-usuario">
            
                <button type="button">
                    <img src="https://www.svgrepo.com/show/532555/search.svg" 
                        alt="Icono de búsqueda"
                        className="button-icono"/>
                </button>

                <button type="button">
                    <img src="https://www.svgrepo.com/show/532468/heart-alt.svg" 
                        alt="Icono de favoritos"
                        className="button-icono"/>
                </button>

                <button type="button">
                    <img src="https://www.svgrepo.com/show/533043/cart-shopping.svg" 
                        alt="Icono de carrito de compras"
                        className="button-icono"/>
                </button>
                
                <button type="button">
                    <img src="https://www.svgrepo.com/show/512729/profile-round-1342.svg" 
                        alt="Icono de perfil de usuario"
                        className="button-icono"/>
                </button>
            

            </div>

        </header>
    )
}