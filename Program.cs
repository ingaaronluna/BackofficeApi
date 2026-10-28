using BackofficeApi.Data;
using BackofficeApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Falta la cadena de conexión DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36)))); // Deben colocar la version de MySQL que tengan instalada en su máquina

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();