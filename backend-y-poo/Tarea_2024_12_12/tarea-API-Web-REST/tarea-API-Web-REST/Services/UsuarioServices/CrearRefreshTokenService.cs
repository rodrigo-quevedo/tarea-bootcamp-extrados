using Configuration;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class CrearRefreshTokenService
    {
        public string CrearRefreshToken(Usuario usuarioValidado, JwtConfiguration jwtConfig)
        {

            SymmetricSecurityKey llavePrivada =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.secret));


            SigningCredentials configuracionFirmaJWT = new SigningCredentials(llavePrivada, SecurityAlgorithms.HmacSha256);

            //claims son una parte de los datos que van a ir dentro del JWT payload 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, usuarioValidado.username),//username como PK en vez de ID
            };

            SecurityToken securityToken = new JwtSecurityToken(
                issuer: jwtConfig.issuer,
                audience: jwtConfig.audience,
                claims: claims,
                expires: DateTime.Now.AddDays(120),
                signingCredentials: configuracionFirmaJWT
            );

            string refreshToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine($"refresh token creado para usuario '{usuarioValidado.username}': {refreshToken}");

            return refreshToken;
        }
    }
}
