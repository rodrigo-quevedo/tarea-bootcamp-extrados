using DAO.Entidades.TorneoEntidades;

namespace DAO.Entidades.Custom
{
    public class InsertCartaMazoDTO : Carta_Del_Mazo
    {
        public string Rol { get; set; }
        public string Fase { get; set; }
    }
}
