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
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear;
using Trabajo_Final.Services.UsuarioServices.Registro;
using Trabajo_Final.utils.Exceptions.BaseException;
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

//Authorization header auth
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

        //Status code de la exception
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        Console.WriteLine($"error: {exceptionHandlerPathFeature?.Error}");

        if (exceptionHandlerPathFeature?.Error is MiExceptionBase exceptionBase)
            context.Response.StatusCode = exceptionBase.ExceptionStatusCode;
            
        else { context.Response.StatusCode = StatusCodes.Status500InternalServerError; }
        

        //HTTP body
        //exception por default
        await context.Response.WriteAsJsonAsync<ResponseBodyDTO>(
            new ResponseBodyDTO(
                exceptionHandlerPathFeature?.Error.Message,
                exceptionHandlerPathFeature?.Error.GetType().Name,
                exceptionHandlerPathFeature?.Path
            )
        );
        
    });
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Console.WriteLine("Despues del app.run() y antes del segundo constructor");
//VerificarExistenciaAdmin crearPrimerAdmin2 = new VerificarExistenciaAdmin();

app.Run();