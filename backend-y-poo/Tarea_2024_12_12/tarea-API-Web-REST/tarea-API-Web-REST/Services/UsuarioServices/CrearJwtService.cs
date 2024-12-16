using Configuration;
using DAO_biblioteca_de_cases.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace tarea_API_Web_REST.Services.UsuarioServices
{
    public class CrearJwtService
    {
        JwtConfiguration jwtConfig;
        public CrearJwtService(JwtConfiguration jwtConfiguration)
        {
            jwtConfig = jwtConfiguration;
        }

        public string CrearJwt(Usuario usuarioValidado, IResponseCookies resCookies)
        {
            //esta llavePrivada se deberia guardar en una variable de entorno
            SymmetricSecurityKey llavePrivada =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.jwt_secret));


            SigningCredentials configuracionFirmaJWT = new SigningCredentials(llavePrivada, SecurityAlgorithms.HmacSha256);

            //Console.WriteLine($"llavePrivada (bytes): ");
            //string llaveString = "";
            //llavePrivada.Key.ToList().ForEach(x => llaveString+=x);
            //Console.WriteLine($"llaveString");
            //Console.WriteLine($"llavePrivada (string): ");
            //Console.WriteLine(Encoding.UTF8.GetString(llavePrivada.Key));
            //Console.WriteLine($"\nAlgorithm: {configuracionFirmaJWT.Algorithm}, " +
            //    $"\n Kid:{configuracionFirmaJWT.Kid}, " +
            //    $"\n Key: {configuracionFirmaJWT.Key}, " +
            //    $"\n Digest: {configuracionFirmaJWT.Digest}, " +
            //    $"\n ToString(): {configuracionFirmaJWT.ToString()}");
            //Console.WriteLine(SecurityAlgorithms.HmacSha256);

            //claims son una parte de los datos que van a ir dentro del JWT payload 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, usuarioValidado.username),//username como PK en vez de ID
                new Claim(ClaimTypes.Name, usuarioValidado.nombre),
                new Claim(ClaimTypes.Role, usuarioValidado.role)
            };


            //JwtSecurityToken hereda de SecurityToken
            //basicamente es un objeto con todo lo que va a ir en el Header y el Payload del jwt
            //Header: saca el algoritmo de SigningCredentials
            //Payload: issuer, audience, expires, y todo lo que haya en claims
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: jwtConfig.issuer,
                audience: jwtConfig.audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: configuracionFirmaJWT
            );

            //Console.WriteLine($"securityToken: {securityToken}");
            //Console.WriteLine($"securityToken.ToString(): {securityToken.ToString()}");

            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine($"jwt creado para usuario '{usuarioValidado.username}': {jwt}");


            //actualizar cookies del header de la response
            resCookies.Append("jwt", jwt, new CookieOptions
             {
                 HttpOnly = false,
                 SameSite = SameSiteMode.None,
                 Secure = true,
                 Expires = DateTime.Now.AddMinutes(5)
             }
                );


            return jwt;

        }
    }
}
