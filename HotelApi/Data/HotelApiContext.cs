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
        public DbSet<HotelApi.Models.ImagenHabitacion> ImagenHabitacion { get; set; } = default!;
        public DbSet<HotelApi.Models.Pension> Pension { get; set; } = default!;
        public DbSet<HotelApi.Models.Reserva> Reserva { get; set; } = default!;
        public DbSet<HotelApi.Models.Cancelacion> Cancelacion { get; set; } = default!;
        public DbSet<HotelApi.Models.Checkin> Checkin { get; set; } = default!;
        public DbSet<HotelApi.Models.Checkout> Checkout { get; set; } = default!;
        public DbSet<HotelApi.Models.Consulta> Consulta { get; set; } = default!;
        public DbSet<HotelApi.Models.DetalleHuesped> DetalleHuesped { get; set; } = default!;
        public DbSet<HotelApi.Models.DetalleReserva> DetalleReserva { get; set; } = default!;
        public DbSet<HotelApi.Models.EstadoReserva> EstadoReserva { get; set; } = default!;
    }
}
