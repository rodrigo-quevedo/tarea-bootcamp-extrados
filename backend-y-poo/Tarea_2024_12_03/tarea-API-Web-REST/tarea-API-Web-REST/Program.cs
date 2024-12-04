using Configuration;
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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:secret"]))
        };
    }
);
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
//patron OPTIONS
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
//builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));

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
