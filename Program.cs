
using API.Helpers;
using Core.Interfaces;
using Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseMySql(connectionString,
                            ServerVersion.AutoDetect(connectionString)));
                            
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILugarRepositorio, LugarRepositorio>();
builder.Services.AddScoped(typeof(IRepositorio<>),(typeof(Repositorio<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));
var app = builder.Build();

//Aplicar las nuevas migraciones al ejecutar la aplicacion y alimentar la base de datos

using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await BaseDatosSeed.SeedAsync(context, loggerFactory);
    }
    catch (System.Exception)
    {
        
        throw;
    }
}
/////////////////////////////////////////////////////////


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
