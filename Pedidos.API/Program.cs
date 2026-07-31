using Microsoft.EntityFrameworkCore;
using Pedidos.API.Middleware;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Services;
using Pedidos.Domain.Interfaces;
using Pedidos.Infrastructure.Data;
using Pedidos.Infrastructure.Repositories;
using Pedidos.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger/OpenAPI

// 3. Configurar DbContext (EF Core)
builder.Services.AddDbContext<PedidosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Inyección de Dependencias
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IExternalValidationService, ExternalValidationService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
// Registramos ILogger para inyección
builder.Services.AddSingleton(Log.Logger);

var app = builder.Build();

// 5. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar el Middleware Global de Excepciones
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
