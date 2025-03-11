using Configuration;
using Configuration.Jwt;
using DAO.DAOs.UsuarioDao;
using DAO.Entidades.UsuarioEntidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear
{
    public class CrearRefreshTokenService : ICrearRefreshTokenService
    {
        IUsuarioDAO usuarioDAO { get; set; }
        IJwtConfiguration jwtConfig { get; set; }

        public CrearRefreshTokenService(IJwtConfiguration jwtConfiguration, IUsuarioDAO usuarioDao)
        {
            jwtConfig = jwtConfiguration;
            usuarioDAO = usuarioDao;
        }


        public string CrearRefreshToken(Usuario usuarioValidado)
        {

            SymmetricSecurityKey llavePrivada =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.refreshToken_secret));


            SigningCredentials configuracionFirmaJWT = new SigningCredentials(llavePrivada, SecurityAlgorithms.HmacSha256);

            //claims son una parte de los datos que van a ir dentro del JWT payload 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, usuarioValidado.Id.ToString()),//username como PK en vez de ID
            };

            SecurityToken securityToken = new JwtSecurityToken(
                issuer: jwtConfig.issuer,
                audience: jwtConfig.audience,
                claims: claims,
                expires: DateTime.Now.AddDays(120),
                signingCredentials: configuracionFirmaJWT
            );

            string refreshToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine($"refresh token creado para usuario '{usuarioValidado.Email}'");



            //devolver refreshToken string
            return refreshToken;
        }
    }
}
