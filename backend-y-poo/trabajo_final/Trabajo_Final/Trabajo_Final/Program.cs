using Configuration;
using Configuration.DI;
using DAO.DAOs;
using DAO.DAOs.DI;
using DAO.Entidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
using Trabajo_Final.Services.UsuarioServices.RefreshToken;
using Trabajo_Final.Services.UsuarioServices.Registro;
using Trabajo_Final.utils.Exceptions.Exceptions;
using Trabajo_Final.utils.Verificar_Existencia_Admin;
using static System.Net.Mime.MediaTypeNames;


var builder = WebApplication.CreateBuilder(args);

// Servicios (auto-inyeccion de dependencias)

builder.Services.AddSingleton<IUsuarioDAO>(
    new UsuarioDAO(builder.Configuration.GetSection(
            "DB:general_connection_string"
        ).Value
    )
);
builder.Services.AddSingleton<IPrimer_AdminConfiguration>(
    new Primer_AdminConfiguration(
        builder.Configuration.GetSection("DB:Primer_Admin:Pais").Value,
        builder.Configuration.GetSection("DB:Primer_Admin:Nombre_apellido").Value,
        builder.Configuration.GetSection("DB:Primer_Admin:Email").Value,
        builder.Configuration.GetSection("DB:Primer_Admin:Password").Value
    )
);
builder.Services.AddSingleton<IJwtConfiguration>(
    new JwtConfiguration(
        builder.Configuration.GetSection("Jwt:jwt_secret").Value,
        builder.Configuration.GetSection("Jwt:refreshToken_secret").Value,
        builder.Configuration.GetSection("Jwt:issuer").Value,
        builder.Configuration.GetSection("Jwt:audience").Value
    )
);
builder.Services.AddSingleton<IVerificarExistenciaAdmin, VerificarExistenciaAdmin>();

//services
builder.Services.AddScoped<ICrearJwtService, CrearJwtService>();
builder.Services.AddScoped<ICrearRefreshTokenService, CrearRefreshTokenService>();
builder.Services.AddScoped<IRegistroUsuarioService, RegistroUsuarioService>();
builder.Services.AddScoped<IValidarRegistroUsuarioService, ValidarRegistroUsuarioService>();
builder.Services.AddScoped<ILogearUsuarioService, LogearUsuarioService>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//jwt auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    Console.WriteLine("dentro de addAuthentication");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:jwt_secret"]))
    };


    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            Console.WriteLine("Jwt recibida");

            //Por defecto mando de vuelta el header (así es más fácil de configurar el front)
            string auth_value = context.Request.Headers.Authorization.ToString();
            context.Response.Headers.Authorization = new StringValues(auth_value);
            //fix para Exceptions
            context.HttpContext.Items["Authorization_value"] = auth_value;
            context.HttpContext.Items["RefreshToken_value"] = context.HttpContext.Request.Cookies["refreshToken"];

            return Task.CompletedTask;
        },

        //OnChallenge: En este punto ya se ejecutó el [Authorize] del controller y
        //se verificó que el Authorization header es inválido.
        //Se realizará la comprobación del refreshToken.
        //Si es válido, se agrega Authorization header actualizado y se
        //hace otra request con el Authentication actualizado.
        //Si es inválido, se eliminan ambos headers (Authorization y la cookie refreshToken), 
        //la request va a seguir de largo y ya queda marcada como no autorizada (ya viene marcada así al OnChallenge).
        OnChallenge = context =>
        {
            Console.WriteLine("JWT Authentication challenge.");

            //comprobar refreshToken
            string refreshToken = context.Request.Cookies["refreshToken"];

            if (refreshToken == null || refreshToken == "") 
            {
                context.Response.Headers.Authorization = "";
                context.Response.Cookies.Append("refreshToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now
                });

                //fix para Exceptions
                context.HttpContext.Items["Authorization_value"] = "";
                context.HttpContext.Items["RefreshToken_value"] = "";

                //Si llega hasta acá, lo más seguro es que es un usuario sin logear (no tiene refreshToken), no hace falta tirar InvalidRefreshTokenException.
                return Task.CompletedTask;
            }

            //hay refreshToken -> verificar si es valido
            JwtSecurityTokenHandler jwtHandler = new();

            //parse refreshToken y verificar formato jwt del refreshToken:
            JwtSecurityToken jwtSecToken;
            try { jwtSecToken = jwtHandler.ReadJwtToken(refreshToken); }
            catch (Exception ex) {
                //se van a borrar en el Exceptions handler
                context.HttpContext.Items["Authorization_value"] = "";
                context.HttpContext.Items["RefreshToken_value"] = "";
                throw new InvalidRefreshTokenException("Token inválido. Logear usuario nuevamente."); 
            }

            //obtener username dentro del refreshToken payload:
            Object id_usuario_out; 
            int id_usuario = 0;
            jwtSecToken.Payload.TryGetValue(ClaimTypes.Sid, out id_usuario_out);
            Int32.TryParse(id_usuario_out?.ToString(), out id_usuario);
            Console.WriteLine($"ID del usuario de la refreshToken: {id_usuario}");

            //buscar usuario con el username del refreshToken en la base de datos:
            UsuarioDAO usuarioDAO = new UsuarioDAO(builder.Configuration.GetSection("DB:general_connection_string").Value);
            Usuario usuarioEncontrado = usuarioDAO.BuscarUnUsuario(new Usuario(id_usuario, true));
            if (usuarioEncontrado == null) throw new InvalidRefreshTokenException("Token inválido. Logear usuario nuevamente.");
            Console.WriteLine($"usuario encontrado para comparar refreshToken: {usuarioEncontrado.Id}|{usuarioEncontrado.Email}|{usuarioEncontrado.Nombre_apellido}|{usuarioEncontrado.Pais}|{usuarioEncontrado.Refresh_token}|{usuarioEncontrado.Id_usuario_creador}");

            //validacion guard-> comprarar refreshToken de la base de datos contra el de la cookie:
            if (usuarioEncontrado.Refresh_token != refreshToken) 
            {
                //se van a borrar en el Exceptions handler
                context.HttpContext.Items["Authorization_value"] = "";
                context.HttpContext.Items["RefreshToken_value"] = "";
                throw new InvalidRefreshTokenException("RefreshToken inválido. Logear usuario nuevamente."); 
            }

            //si es valido, crear un jwt
            string jwtCreado = new CrearJwtService(new JwtConfiguration(
                builder.Configuration.GetSection("Jwt:jwt_secret").Value,
                builder.Configuration.GetSection("Jwt:refreshToken_secret").Value,
                builder.Configuration.GetSection("Jwt:issuer").Value,
                builder.Configuration.GetSection("Jwt:audience").Value
            ))
            .CrearJwt(usuarioEncontrado);

            //actualizar Authorization 
            context.Response.Headers.Authorization = new StringValues($"Bearer {jwtCreado}");

            //fix para Exceptions
            context.HttpContext.Items["Authorization_value"] = $"Bearer {jwtCreado}";
            context.HttpContext.Items["RefreshToken_value"] = refreshToken;

            ////intenté marcar la request como autorizada (al cargar los Claims en HttpContext.User)
            ////pero cuando se llega a OnChallenge, el servidor ya
            ////ejecutó el [Authorize] en el controller y tiró error 401.
            //context.HttpContext.User = jwtHandler.ValidateToken(
            //    jwtCreado,
            //    new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidIssuer = builder.Configuration["Jwt:issuer"],

            //        ValidateAudience = true,
            //        ValidAudience = builder.Configuration["Jwt:audience"],

            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero,

            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:jwt_secret"]))
            //    },
            //    out SecurityToken validatedToken
            //  );


            //solicitar nueva request
            //-->si el front usa fetch, simplemente hay que volver a hacer un fetch cargando los mismos headers y body
            //-->ese segundo fetch si o si va a ser valido, ya que tiene los tokens correctos (recien traidos del servidor)
            context.Response.Headers["X-Actualizar-Token"] = "cualquier valor";//aca se puede poner cualquier valor, total en el front se hace un (X-Actualizar-Token !== null && X-Actualizar-Token !== '')
            context.HttpContext.Items["X-Actualizar-Token_value"] = "asdfjlks";//fix para Exceptions


            return Task.CompletedTask;
        }


    };

}
);

builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                //Obligatorio usar 'Origin' en las requests
                policy.WithOrigins("*");
            }
        );
    }
);

// Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//error middleware:
app.UseExceptionHandler(exceptionHandlerApp => {
    exceptionHandlerApp.Run(async context => {
        //HTTP headers
        context.Response.ContentType = MediaTypeNames.Application.Json;

        //Esto es un fix para que las Exceptions no me sobreescriban el Authorization header
        //Esta API siempre envía el Authorization header actualizado en sus Response,
        //de esta forma el frontend solo tiene que actualizar el Authorization header con lo que le llega del servidor
        //string authorization_value = "";
        //if (context.Items.TryGetValue("Authorization_value", out var auth_outObj)) authorization_value = auth_outObj as string;
        //context.Response.Headers.Authorization = new StringValues(authorization_value);

        ////fix para refreshToken
        //string refreshToken_value = "";
        //if (context.Items.TryGetValue("RefreshToken_value", out var refreshToken_outObj)) refreshToken_value = refreshToken_outObj as string;
        //context.Response.Cookies.Append("refreshToken", refreshToken_value, new CookieOptions
        //{
        //    HttpOnly = true,
        //    SameSite = SameSiteMode.None,
        //    Secure = true,
        //    Expires = DateTime.Now.AddDays(120)
        //});

        ////fix para X-Actualizar-Token (este header pide re-hacer la request con el token actualizado)
        //string actualizar_value = "";
        //if (context.Items.TryGetValue("X-Actualizar-Token_value", out var actualizar_outObj)) actualizar_value = actualizar_outObj as string;
        //context.Response.Headers["X-Actualizar-Token_value"] = new StringValues(actualizar_value);



        //HTTP body
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        Console.WriteLine($"error: {exceptionHandlerPathFeature?.Error}");
        //if (exceptionHandlerPathFeature?.Error == null) { Console.WriteLine("error is NULL"); }
        //else Console.WriteLine("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");

        //exceptions específicas
        if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(InvalidCredentialsException).Name) context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(NotFoundException).Name) context.Response.StatusCode = StatusCodes.Status404NotFound;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(AlreadyExistsException).Name) context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(SinPermisoException).Name) context.Response.StatusCode = StatusCodes.Status403Forbidden;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(AlreadyLoggedInException).Name) context.Response.StatusCode = StatusCodes.Status403Forbidden;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(InvalidRefreshTokenException).Name) context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        
        else { context.Response.StatusCode = StatusCodes.Status500InternalServerError; }



        //guarda, el .WriteAsync es incompatible con el .WriteAsJsonAsync<> 
        //await context.Response.WriteAsync($" Path: {exceptionHandlerPathFeature?.Path}.");


        //Authorization jwt expirados
        //if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(SecurityTokenExpiredException).Name)
        //{
        //    //comprobar refreshToken
        //    string refreshToken = context.Request.Cookies["refreshToken"];

        //    if (refreshToken == null || refreshToken == "")
        //    {
        //        context.Response.Headers.Authorization = "";
        //        context.Response.Cookies.Append("refreshToken", "", new CookieOptions
        //        {
        //            HttpOnly = true,
        //            SameSite = SameSiteMode.None,
        //            Secure = true,
        //            Expires = DateTime.Now
        //        });
        //        throw new InvalidRefreshTokenException("Token inválido. Logear usuario nuevamente.");
        //    }

        //    //hay refreshToken -> verificar si es valido
        //    JwtSecurityTokenHandler jwtHandler = new();

        //    //parse refreshToken y verificar formato jwt del refreshToken:
        //    JwtSecurityToken jwtSecToken;
        //    try { jwtSecToken = jwtHandler.ReadJwtToken(refreshToken); }
        //    catch (Exception ex)
        //    {
        //        context.Response.Headers.Authorization = "";
        //        context.Response.Cookies.Append("refreshToken", "", new CookieOptions
        //        {
        //            HttpOnly = true,
        //            SameSite = SameSiteMode.None,
        //            Secure = true,
        //            Expires = DateTime.Now
        //        });
        //        throw new InvalidRefreshTokenException("Token inválido. Logear usuario nuevamente.");
        //    }

        //    //obtener username dentro del refreshToken payload:
        //    Object id_usuario_out;
        //    int id_usuario = 0;
        //    jwtSecToken.Payload.TryGetValue(ClaimTypes.Sid, out id_usuario_out);
        //    Int32.TryParse(id_usuario_out?.ToString(), out id_usuario);
        //    Console.WriteLine($"ID del usuario de la refreshToken: {id_usuario}");

        //    //buscar usuario con el username del refreshToken en la base de datos:
        //    UsuarioDAO usuarioDAO = new UsuarioDAO(builder.Configuration.GetSection("DB:general_connection_string").Value);
        //    Usuario usuarioEncontrado = usuarioDAO.BuscarUnUsuario(new Usuario(id_usuario, true));
        //    if (usuarioEncontrado == null) throw new InvalidRefreshTokenException("RefreshToken inválido. Logear usuario nuevamente.");
        //    Console.WriteLine($"usuario encontrado para comparar refreshToken: {usuarioEncontrado.Id}|{usuarioEncontrado.Email}|{usuarioEncontrado.Nombre_apellido}|{usuarioEncontrado.Pais}|{usuarioEncontrado.Refresh_token}|{usuarioEncontrado.Id_usuario_creador}");

        //    //validacion guard-> comprarar refreshToken de la base de datos contra el de la cookie:
        //    if (usuarioEncontrado.Refresh_token != refreshToken)
        //    {
        //        context.Response.Headers.Authorization = "";
        //        context.Response.Cookies.Append("refreshToken", "", new CookieOptions
        //        {
        //            HttpOnly = true,
        //            SameSite = SameSiteMode.None,
        //            Secure = true,
        //            Expires = DateTime.Now
        //        });
        //        throw new InvalidRefreshTokenException("RefreshToken inválido. Logear usuario nuevamente.");
        //    }

        //    //si es valido, crear un jwt
        //    string jwtCreado = new CrearJwtService(new JwtConfiguration(
        //        builder.Configuration.GetSection("Jwt:jwt_secret").Value,
        //        builder.Configuration.GetSection("Jwt:refreshToken_secret").Value,
        //        builder.Configuration.GetSection("Jwt:issuer").Value,
        //        builder.Configuration.GetSection("Jwt:audience").Value
        //    ))
        //    .CrearJwt(usuarioEncontrado);

        //    //actualizar Authorization 
        //    context.Response.Headers.Authorization = new StringValues($"Bearer {jwtCreado}");
        //}



        //exception por default
        await context.Response.WriteAsJsonAsync<ResponseBodyDTO>(
            new ResponseBodyDTO(
                exceptionHandlerPathFeature?.Error.Message,
                exceptionHandlerPathFeature?.Error.GetType().Name,
                exceptionHandlerPathFeature?.Path
            )
        );
        
        //await context.Response.WriteAsync(
        //    $"{exceptionHandlerPathFeature?.Error.Message} . \nException type: {exceptionHandlerPathFeature?.Error?.GetType()}"
        //);
    });
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Console.WriteLine("Despues del app.run() y antes del segundo constructor");
//VerificarExistenciaAdmin crearPrimerAdmin2 = new VerificarExistenciaAdmin();

app.Run();