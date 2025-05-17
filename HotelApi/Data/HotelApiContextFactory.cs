using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using HotelApi.Data;

namespace HotelApi.Data
{
    public class HotelApiContextFactory : IDesignTimeDbContextFactory<HotelApiContext>
    {
        public HotelApiContext CreateDbContext(string[] args)
        {
            // Construye la configuración leyendo appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ubicación base para buscar el appsettings
                .AddJsonFile("appsettings.json") // Asegúrate de tener este archivo con la connection string
                .Build();

            // Lee la connection string desde appsettings.json
            var connectionString = configuration.GetConnectionString("HotelApiContext");

            var optionsBuilder = new DbContextOptionsBuilder<HotelApiContext>();
            optionsBuilder.UseSqlServer(connectionString); // Usa el proveedor correcto (en este caso SQL Server)

            return new HotelApiContext(optionsBuilder.Options);
        }
    }
}
