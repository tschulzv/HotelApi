using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HotelApi.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotelApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelApiContext") ?? throw new InvalidOperationException("Connection string 'HotelApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ejecutar el seed de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HotelApiContext>();
    DbInitializer.Seed(context);
}

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
