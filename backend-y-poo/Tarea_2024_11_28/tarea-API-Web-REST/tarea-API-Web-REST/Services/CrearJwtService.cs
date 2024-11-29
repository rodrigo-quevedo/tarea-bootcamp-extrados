using DAO_biblioteca_de_cases.Entidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace tarea_API_Web_REST.Services
{
    public class CrearJwtService
    {

        public string CrearJwt(Usuario usuarioValidado)
        {
            //esta llavePrivada se deberia guardar en una variable de entorno
            SymmetricSecurityKey llavePrivada = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("passwordpasswordpasswordpasswordpasswordpasswordpassword"));


            SigningCredentials configuracionFirmaJWT = new SigningCredentials(llavePrivada, SecurityAlgorithms.HmacSha256);
            
            //Console.WriteLine($"llavePrivada: {llavePrivada}");
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
                new Claim(ClaimTypes.Name, usuarioValidado.nombre)
            };


            //JwtSecurityToken hereda de SecurityToken
            //basicamente es un objeto con todo lo que va a ir en el Header y el Payload del jwt
            //Header: saca el algoritmo de SigningCredentials
            //Payload: issuer, audience, expires, y todo lo que haya en claims
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: "http://localhost:5176",
                audience : "http://localhost:5176",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: configuracionFirmaJWT
            );

            //Console.WriteLine($"securityToken: {securityToken}");
            //Console.WriteLine($"securityToken.ToString(): {securityToken.ToString()}");

            string jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine($"jwt creado para usuario '{usuarioValidado.username}': {jwt}");

            return jwt;
        }
    }
}
