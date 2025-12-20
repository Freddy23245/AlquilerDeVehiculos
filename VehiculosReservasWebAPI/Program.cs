using Microsoft.EntityFrameworkCore;
using VehiculosReservasWebAPI.Repositorio;
using VehiculosReservasWebAPI.Repositorio.IRepositorio;
using VehiculosReservasWebAPI.ReservasMappers;
using VehiculosReservasWebAPI.Services;
using VehiculosReservasWebAPI.Services.IService;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VehiculosReservasWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var claveSecreta = Encoding.ASCII.GetBytes("clave_secreta_aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"); // Cambia esto con una clave segura

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(claveSecreta)
        };
    });


builder.Services.AddDbContext<ReservasCocheraContext>(
    opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));
// Add services to the container.
//builder.Services.AddAutoMapper(typeof(VehiculosMappers).Assembly);
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<VehiculosMappers>();
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IAlquilerRepository, AlquilerRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
