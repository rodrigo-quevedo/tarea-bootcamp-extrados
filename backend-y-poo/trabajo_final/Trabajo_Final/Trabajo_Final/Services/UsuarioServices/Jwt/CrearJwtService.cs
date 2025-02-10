using Configuration;
using Configuration.DI;
using DAO.Entidades.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Trabajo_Final.Services.UsuarioServices.Jwt
{
    public class CrearJwtService : ICrearJwtService
    {
        IJwtConfiguration jwtConfig;
        public CrearJwtService(IJwtConfiguration jwtConfiguration)
        {
            jwtConfig = jwtConfiguration;
        }

        


        public string CrearJwt(Usuario usuarioValidado)
        {
            //jwt private key
            SymmetricSecurityKey llavePrivada =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.jwt_secret));

            //algoritmo hasheo
            SigningCredentials configuracionFirmaJWT = 
                new SigningCredentials(llavePrivada, SecurityAlgorithms.HmacSha256);

            //payload
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, usuarioValidado.Id.ToString()),
                new Claim(ClaimTypes.Email, usuarioValidado.Email),
                new Claim(ClaimTypes.Role, usuarioValidado.Rol)
            };


            //crear jwt y completar payload
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: jwtConfig.issuer,
                audience: jwtConfig.audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: configuracionFirmaJWT
            );
            
            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine($"jwt creado para usuario '{usuarioValidado.Email}'");


            return jwt;

        }
    }
}
