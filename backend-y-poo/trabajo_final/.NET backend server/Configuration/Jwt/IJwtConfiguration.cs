using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Jwt
{
    public interface IJwtConfiguration
    {
        public string jwt_secret { get; set; }
        public string refreshToken_secret { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
    }
}
