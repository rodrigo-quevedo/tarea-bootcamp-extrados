using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Console.WriteLine("inicio programa");


//built in
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5176",

            ValidateAudience = true,
            ValidAudience = "http://localhost:5176",

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("passwordpasswordpasswordpasswordpasswordpasswordpassword"))
        };
    });
builder.Services.AddCors(
    options =>
    {
        options.AddDefaultPolicy(
            policy =>
            {
                //SOLAMENTE CUANDO COINCIDE se devuelve el header "Access-Control-Allow-Origin" 
                //si el header "Origin" no se encuentra en la request, no se devuelve el header "Access-Control-Allow-Origin" 
                //si el header "Origin" no coincide con los origin permitidos, tampoco se devuelve el header "Access-Control-Allow-Origin" 
                policy.WithOrigins("*");
                //policy.AllowAnyOrigin();//esta es la alternativa
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//aca se aplican los middlewares (hay que respetar el orden de middlewares)
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
