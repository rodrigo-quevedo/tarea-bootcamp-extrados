using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Entidades
{
    public class Refresh_Token
    {
        public int id_usuario {  get; set; } 
        public string refresh_token { get; set; }
        public bool token_activo {  get; set; }
    }
}
