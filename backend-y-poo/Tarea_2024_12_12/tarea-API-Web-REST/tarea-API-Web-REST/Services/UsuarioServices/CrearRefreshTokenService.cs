using Configuration;
using DAO_biblioteca_de_cases.DAOs;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class CrearRefreshTokenService
    {
        UsuarioDAO usuarioDAO { get; set; }
        JwtConfiguration jwtConfig {  get; set; }

        public CrearRefreshTokenService(JwtConfiguration jwtConfiguration, string connectionString)
        {
            usuarioDAO = new UsuarioDAO(connectionString);
            jwtConfig = jwtConfiguration;
        }


        public string CrearRefreshToken(Usuario usuarioValidado, IResponseCookies resCookies)
        {

            SymmetricSecurityKey llavePrivada =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.refreshToken_secret));


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

            //guardar refresh token en db
            usuarioDAO.AsignarRefreshTokenByUsername(usuarioValidado.username, refreshToken);

            //actualizar cookies header
            resCookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.Now.AddDays(120)
            });

            //devolver refreshToken string
            return refreshToken;
        }
    }
}
