import Header from "./components/Header"
import Footer from "./components/Footer"
import Newsletter from "./components/Newsletter"
import Categorias from "./components/Categorias"
import Novedades from "./components/Novedades"

function App() {
  

  return (
    <>
        <Header/>

        <main>
            <Newsletter />

            <h1>Tu tienda de ropa online</h1>

            <Categorias/>

            <Novedades/>

        </main>

        <Footer/>
    </>
  )
}

export default App
