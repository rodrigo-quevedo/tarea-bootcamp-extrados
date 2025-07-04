using Configuration.FilesPathConfiguration;
using Configuration.Jwt;
using Configuration.Primer_Admin;
using Configuration.ServerRoutes;
using Configuration.ServerRoutes.RoutesParam;
using Configuration.ServerURL;
using Custom_Exceptions.Exceptions.BaseException;
using Custom_Exceptions.Exceptions.Exceptions;
using DAO.DAOs.Cartas;
using DAO.DAOs.Partidas;
using DAO.DAOs.Torneos;
using DAO.DAOs.UsuarioDao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;
using System.Text;
using Trabajo_Final.DTO.Response.Server_HTTP_Response;
using Trabajo_Final.Services.CartasServices.BuscarCartas;
using Trabajo_Final.Services.CartasServices.BuscarSeries;
using Trabajo_Final.Services.ImagenesServices.BuscarImagenes;
using Trabajo_Final.Services.JugadorServices.BuscarPartidas;
using Trabajo_Final.Services.JugadorServices.ColeccionarCartas;
using Trabajo_Final.Services.JugadorServices.ObtenerColeccion;
using Trabajo_Final.Services.JugadorServices.QuitarCartas;
using Trabajo_Final.Services.PartidaServices.ArmarPartidasService;
using Trabajo_Final.Services.PartidaServices.Buscar_Datos_Partidas;
using Trabajo_Final.Services.PartidaServices.Buscar_Partidas_Para_Oficializar;
using Trabajo_Final.Services.PartidaServices.Editar_Jueces_Partida;
using Trabajo_Final.Services.PartidaServices.Editar_Jugadores_Partidas;
using Trabajo_Final.Services.PartidaServices.Oficializar_Partidas;
using Trabajo_Final.Services.TorneoServices.BuscarTorneos;
using Trabajo_Final.Services.TorneoServices.CancelarTorneo;
using Trabajo_Final.Services.TorneoServices.Crear;
using Trabajo_Final.Services.TorneoServices.EditarJueces;
using Trabajo_Final.Services.TorneoServices.IniciarTorneo;
using Trabajo_Final.Services.TorneoServices.InscribirJugador;
using Trabajo_Final.Services.TorneoServices.ValidarJueces;
using Trabajo_Final.Services.UsuarioServices.Buscar;
using Trabajo_Final.Services.UsuarioServices.Editar;
using Trabajo_Final.Services.UsuarioServices.Eliminar;
using Trabajo_Final.Services.UsuarioServices.Jwt;
using Trabajo_Final.Services.UsuarioServices.Login;
using Trabajo_Final.Services.UsuarioServices.Perfil;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.AsignarRefreshToken;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Crear;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Desactivar;
using Trabajo_Final.Services.UsuarioServices.RefreshToken.Validar;
using Trabajo_Final.Services.UsuarioServices.Registro;
using Trabajo_Final.utils.Generar_Cartas;
using Trabajo_Final.utils.Verificar_Existencia_Admin;


var builder = WebApplication.CreateBuilder(args);


// Servicios (auto-inyeccion de dependencias)

builder.Services.AddSingleton<IUsuarioDAO>(
    new UsuarioDAO(builder.Configuration.GetSection(
            "DB:general_connection_string"
        ).Value
    )
);
builder.Services.AddSingleton<ICartaDAO>(
    new CartaDAO(builder.Configuration.GetSection(
            "DB:general_connection_string"
        ).Value
    )
);
builder.Services.AddSingleton<ITorneoDAO>(
    new TorneoDAO(builder.Configuration.GetSection(
            "DB:general_connection_string"
        ).Value
    )
);
builder.Services.AddSingleton<IPartidaDAO>(
    new PartidaDAO(builder.Configuration.GetSection(
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
builder.Services.AddSingleton<VerificarExistenciaAdmin>();
builder.Services.AddSingleton<GenerarCartasYSeries>();


//services
builder.Services.AddScoped<ICrearJwtService, CrearJwtService>();

builder.Services.AddScoped<ILogearUsuarioService, LogearUsuarioService>();

builder.Services.AddScoped<IActualizarJWTService, ActualizarJWTService>();
builder.Services.AddScoped<IAsignarRefreshTokenService, AsignarRefreshTokenService>();
builder.Services.AddScoped<ICrearRefreshTokenService, CrearRefreshTokenService>();
builder.Services.AddScoped<IDesactivarRefreshTokenService, DesactivarRefreshTokenService>();

builder.Services.AddScoped<IRegistroUsuarioService, RegistroUsuarioService>();

builder.Services.AddScoped<IActualizarPerfilService, ActualizarPerfilService>();

builder.Services.AddScoped<IColeccionarCartasService, ColeccionarCartasService>();
builder.Services.AddScoped<IObtenerColeccionService, ObtenerColeccionService>();
builder.Services.AddScoped<IQuitarCartasService,  QuitarCartasService>();

builder.Services.AddScoped<ICrearTorneoService, CrearTorneoService>();
builder.Services.AddScoped<IValidarJuecesService, ValidarJuecesService>();
builder.Services.AddScoped<IAgregarJuezService, AgregarJuezService>();
builder.Services.AddScoped<IEliminarJuezService, EliminarJuezService>();
builder.Services.AddScoped<IBuscarTorneosService, BuscarTorneosService>();
builder.Services.AddScoped<IIniciarTorneoService, IniciarTorneoService>();

builder.Services.AddScoped<IArmarPartidasService, ArmarPartidasService>();

builder.Services.AddScoped<IInscribirJugadorService, InscribirJugadorService>();

builder.Services.AddScoped<IBuscarPartidasParaOficializarService, BuscarPartidasParaOficializarService>();
builder.Services.AddScoped<IOficializarPartidaService, OficializarPartidaService>();

builder.Services.AddScoped<IBuscarPartidasJugadorService, BuscarPartidasJugadorService>();

builder.Services.AddScoped<IEditarJuezPartidaService, EditarJuezPartidaService>();
builder.Services.AddScoped<IEditarJugadoresPartidasService, EditarJugadoresPartidasService>();

builder.Services.AddScoped<ICancelarTorneoService, CancelarTorneoService>();

builder.Services.AddScoped<IEliminarUsuarioService, EliminarUsuarioService>();
builder.Services.AddScoped<IEditarUsuarioService, EditarUsuarioService>();

builder.Services.AddScoped<IBuscarUsuarioService, BuscarUsuarioService>();

builder.Services.AddScoped<IBuscarCartasService, BuscarCartasService>();
builder.Services.AddScoped<IBuscarSeriesService, BuscarSeriesService>();
builder.Services.AddScoped<IBuscarPartidasService, BuscarPartidasService>();

builder.Services.AddSingleton<IBuscarImagenesService, BuscarImagenesService>();

builder.Services.AddSingleton<IServerRoutesConfiguration>(//Se usa para POST url route en DB
    new ServerRoutesConfiguration(
        new Routes()
        {
            Ilustraciones = builder.Configuration.GetSection("Server_Routes:ilustraciones").Value,
            Foto_Perfil = builder.Configuration.GetSection("Server_Routes:foto_perfil").Value
        }
    )
);

builder.Services.AddSingleton<IServerURLConfiguration>(//Se usa para GET archivos del server
    new ServerURLConfiguration(builder.Configuration.GetSection("Server_URL").Value)
);

builder.Services.AddSingleton<IFilesPathsConfigurations>(
    new FilesPathConfiguration(
        builder.Configuration.GetSection("FilesPath:imagenes:ilustraciones").Value,
        builder.Configuration.GetSection("FilesPath:imagenes:foto_perfil").Value
    )
);


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
                policy.WithHeaders("*");
                policy.WithMethods("*");

                //TO DO: Configurarlo mejor
                //policy
                //.WithOrigins("http://localhost:5173")
                //.WithHeaders("Content-Type", "Authorization")
                //.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
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


        //Manejo de custom exceptions
        switch (exceptionHandlerPathFeature?.Error.GetType().Name)
        { 
            case nameof(InvalidRefreshTokenException): //Borrar cookie refreshToken invalida
            {
                context.Response.Cookies.Append("refreshToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now
                });
                break;
            }

            case nameof(MultipleInvalidInputException): //Borrar cookie refreshToken invalida
            {
                if (exceptionHandlerPathFeature?.Error is MultipleInvalidInputException multipleInvalidInputEx)
                    {
                        await context.Response.WriteAsJsonAsync(
                            new {
                                errors = multipleInvalidInputEx.errores,
                                exceptionHandlerPathFeature?.Error.GetType().Name,
                                exceptionHandlerPathFeature?.Path
                            }
                         );
                    }
                break;
            }

        }
        
        
        //HTTP body
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