using Configuration;
using DAO.DAOs;
using DAO.DAOs.DI;
using DAO.Entidades;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Verificar_Existencia_Admin;





var builder = WebApplication.CreateBuilder(args);

// AÑADIDO: Verificar que exista primer admin y crearlo si no existe

VerificarExistenciaAdmin crearPrimerAdmin = new VerificarExistenciaAdmin();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AÑADIDO: patron OPTIONS e Inyeccion de Dependencias

builder.Services.Configure<DBConfiguration>(
    builder.Configuration.GetSection("DB:general_connection_string"));
builder.Services.Configure<Primer_AdminConfiguration>(
    builder.Configuration.GetSection("DB:Primer_Admin"));
//builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();

builder.Services.AddSingleton<IUsuarioDAO>(
    new UsuarioDAO(builder.Configuration.GetSection(
            "DB:general_connection_string"
        ).Value
    )
);




// Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
