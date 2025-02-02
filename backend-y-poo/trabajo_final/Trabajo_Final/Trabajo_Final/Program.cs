using Configuration;
using Configuration.DI;
using DAO.DAOs;
using DAO.DAOs.DI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using System.Text;
using Trabajo_Final.DTO;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
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
builder.Services.AddScoped<ILogearUsuarioService, LogearUsuarioService>();
builder.Services.AddScoped<IJugadorAutoregistroService, JugadorAutoregistroService>();
builder.Services.AddScoped<ICrearJwtService, CrearJwtService>();



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

            //Por defecto mando de vuelta el header (as� es m�s f�cil de configurar el front)
            string value = context.Request.Headers.Authorization.ToString();
            context.Response.Headers.Authorization = new StringValues(value);
            //fix para Exceptions
            context.HttpContext.Items["Authorization_value"] = value;

            return Task.CompletedTask;
        }
        //,

        //OnChallenge = context =>
        //{
        //    Console.WriteLine("JWT Authentication challenge.");

        //    //Por defecto mando de vuelta el header (as� es m�s f�cil de configurar el front)
        //    string value = context.Request.Headers.Authorization.ToString();
        //    context.Response.Headers.Authorization = new StringValues(value);
        //    //fix para Exceptions
        //    context.HttpContext.Items["Authorization_value"] = value;

        //    return Task.CompletedTask;
        //},

        //OnTokenValidated = context =>
        //{
        //    Console.WriteLine("Token valido");

        //    //Por defecto mando de vuelta el header (as� es m�s f�cil de configurar el front)
        //    string value = context.Request.Headers.Authorization.ToString();
        //    context.Response.Headers.Authorization = new StringValues(value);
        //    //fix para Exceptions
        //    context.HttpContext.Items["Authorization_value"] = value;

        //    return Task.CompletedTask;
        //},

        //OnAuthenticationFailed = context =>
        //{
        //    Console.WriteLine("Auth Failed");

        //    //Por defecto mando de vuelta el header (as� es m�s f�cil de configurar el front)
        //    string value = context.Request.Headers.Authorization.ToString();
        //    context.Response.Headers.Authorization = new StringValues(value);
        //    //fix para Exceptions
        //    context.HttpContext.Items["Authorization_value"] = value;

        //    return Task.CompletedTask;
        //},

        //OnForbidden = context =>
        //{
        //    Console.WriteLine("Auth OnForbidden");

        //    //Por defecto mando de vuelta el header (as� es m�s f�cil de configurar el front)
        //    string value = context.Request.Headers.Authorization.ToString();
        //    context.Response.Headers.Authorization = new StringValues(value);
        //    //fix para Exceptions
        //    context.HttpContext.Items["Authorization_value"] = value;

        //    return Task.CompletedTask;
        //}


    };

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
        //Esta API siempre env�a el Authorization header actualizado en sus Response,
        //de esta forma el frontend solo tiene que actualizar el Authorization header con lo que le llega del servidor
        string authorization_value = "";
        if (context.Items.TryGetValue("Authorization_value", out var value)) authorization_value = value as string;
        context.Response.Headers.Authorization = new StringValues(authorization_value);

        //HTTP body
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        Console.WriteLine($"error: {exceptionHandlerPathFeature?.Error}");
        //if (exceptionHandlerPathFeature?.Error == null) { Console.WriteLine("error is NULL"); }
        //else Console.WriteLine("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");

        //exceptions espec�ficas
        if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(InvalidCredentialsException).Name) context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(NotFoundException).Name) context.Response.StatusCode = StatusCodes.Status404NotFound;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(AlreadyExistsException).Name) context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(SinPermisoException).Name) context.Response.StatusCode = StatusCodes.Status403Forbidden;
        else if (exceptionHandlerPathFeature?.Error.GetType().Name == typeof(AlreadyLoggedInException).Name) context.Response.StatusCode = StatusCodes.Status403Forbidden;
        else { context.Response.StatusCode = StatusCodes.Status500InternalServerError; }



        //guarda, el .WriteAsync es incompatible con el .WriteAsJsonAsync<> 
        //await context.Response.WriteAsync($" Path: {exceptionHandlerPathFeature?.Path}.");





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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Console.WriteLine("Despues del app.run() y antes del segundo constructor");
//VerificarExistenciaAdmin crearPrimerAdmin2 = new VerificarExistenciaAdmin();

app.Run();