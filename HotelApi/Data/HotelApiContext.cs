using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelApi.Models;

namespace HotelApi.Data
{
    public class HotelApiContext : DbContext
    {
        public HotelApiContext (DbContextOptions<HotelApiContext> options)
            : base(options)
        {
        }

        public DbSet<HotelApi.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<HotelApi.Models.Habitacion> Habitacion { get; set; } = default!;
    }
}
