

export default function Newsletter() {
    return (
        <section className="newsletter">
            <div className="newsletter__container">

                <p>Suscríbase al newsletter semanal de ofertas</p>
                
                <form method="post" action="#" className="newsletter__form">
                    <input type="email" 
                        name="email_suscripto_newsletter"
                        placeholder="email"/>
                    <button type="submit">
                    <img src="https://www.svgrepo.com/show/533310/send-alt-1.svg" alt="Boton de envio de formulario"/>
                    </button>
                </form>

            </div>
        </section>
    )
}