using DAO.Entidades;
using Trabajo_Final.utils.Constantes;
using Trabajo_Final.utils.Verificar_Existencia_Admin;





var builder = WebApplication.CreateBuilder(args);

// AÑADIDO: Verificar que exista primer admin y crearlo si no existe

VerificarExistenciaAdmin crearPrimerAdmin = new VerificarExistenciaAdmin(
    builder.Configuration["DB:general_connection_string"],
    new Usuario(
        0,
        Roles.ADMIN,
        builder.Configuration["DB:Primer_Admin:Pais"],
        builder.Configuration["DB:Primer_Admin:Nombre_apellido"],
        builder.Configuration["DB:Primer_Admin:Email"],
        builder.Configuration["DB:Primer_Admin:Password"],
        true
    )
);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
