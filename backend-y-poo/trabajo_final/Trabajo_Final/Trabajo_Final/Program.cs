using Configuration;
using Configuration.DI;
using DAO.DAOs;
using DAO.DAOs.DI;
using Trabajo_Final.utils.Verificar_Existencia_Admin;


var builder = WebApplication.CreateBuilder(args);

// Servicios, patron OPTIONS e Inyeccion de Dependencias
builder.Services.Configure<DBConfiguration>(
    builder.Configuration.GetSection("DB:general_connection_string"));
builder.Services.Configure<Primer_AdminConfiguration>(
    builder.Configuration.GetSection("DB:Primer_Admin"));


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
builder.Services.AddSingleton<IVerificarExistenciaAdmin, VerificarExistenciaAdmin>();




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

//Console.WriteLine("Despues del app.run() y antes del segundo constructor");
//VerificarExistenciaAdmin crearPrimerAdmin2 = new VerificarExistenciaAdmin();

app.Run();