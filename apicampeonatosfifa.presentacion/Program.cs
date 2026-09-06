using apicampeonatosfifa.core;
using apicampeonatosfifa.core.repositorios;
using apicampeonatosfifa.core.servicios;
using apicampeonatosfifa.infraestructura;
using apicampeonatosfifa.infraestructura.Persistencia;
using apicampeonatosfifa.infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAR CONTROLADORES
builder.Services.AddControllers();

// 2. REGISTRAR LAS CONEXIONES A LAS BASES DE DATOS
// Base de datos del profesor (Campeonatos)
builder.Services.AddDbContext<CampeonatosFIFAContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Base de datos tuya (Festivos)
builder.Services.AddDbContext<FestivosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FestivosConnection")));

// 3. REGISTRAR REPOSITORIOS
builder.Services.AddScoped<IPaisRepositorio, PaisRepositorio>();
builder.Services.AddScoped<ITipoRepositorio, TipoRepositorio>();
builder.Services.AddScoped<IFestivoRepositorio, FestivoRepositorio>();
builder.Services.AddScoped<ISeleccionRepositorio, SeleccionRepositorio>(); // <-- Registramos el de selecciones del profesor

// 4. REGISTRAR SERVICIO DE CALENDARIO
builder.Services.AddScoped<ICalendarioServicio, CalendarioServicio>();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 5. ENLAZAR RUTAS
app.UseAuthorization();
app.MapControllers();

// --- PRUEBA ORIGINAL CLIMA ---
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}