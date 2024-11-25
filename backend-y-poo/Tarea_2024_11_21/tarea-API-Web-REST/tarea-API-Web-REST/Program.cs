using DAO_biblioteca_de_cases;
using DAO_biblioteca_de_cases.Entidades;

Console.WriteLine("inicio programa");

//test DAO
UsuarioDAO dao = new UsuarioDAO();
//Usuario u1 = dao.CrearUsuario("hola@gmail.com", "hola", 20);
//if (u1 != null) u1.mostrarDatos();
//else Console.WriteLine("u1 is null");
//Usuario u2 = dao.BuscarUsuarioPorMail("hola@gmail.com");
//if (u2 != null) u2.mostrarDatos();
//else Console.WriteLine("u2 is null");
//Usuario u3 = dao.ActualizarUsuario("hola@gmail.com", "eduardo", 30);
//if (u3 != null)
//{
//    Console.WriteLine("Despues del update:");
//    u3.mostrarDatos();
//}
//else Console.WriteLine("u3 is null");


//built in
var builder = WebApplication.CreateBuilder(args);

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
