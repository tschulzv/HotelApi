using HotelApi.Models;
using System.Collections.ObjectModel;

namespace HotelApi.Data
{
    public static class DbInitializer
    {
        public static void Seed(HotelApiContext context)
        {
            context.Database.EnsureCreated();

            var now = DateTime.Now;
         

            // Verificamos si ya existen datos
            if (context.Servicio.Any() ||
                context.TipoDocumento.Any() ||
                context.TipoHabitacion.Any() ||
                context.Pension.Any() ||
                context.ImagenHabitacion.Any())
            { 
                return;
            }


            // Seed Servicios
            var servicios = new List<Servicio>
            {
                new Servicio { Nombre = "WiFi", IconName = "wifi", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Piscina", IconName = "pool", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Estacionamiento", IconName = "local_parking", Creacion = now, Actualizacion = now, Activo = true }
            };
            context.Servicio.AddRange(servicios);

            // Seed TipoDocumento
            var tiposDocumento = new List<TipoDocumento>
            {
                new TipoDocumento { Nombre = "DNI", Creacion = now, Actualizacion = now, Activo = true },
                new TipoDocumento { Nombre = "Pasaporte", Creacion = now, Actualizacion = now, Activo = true },
                new TipoDocumento { Nombre = "Carnet de extranjería", Creacion = now, Actualizacion = now, Activo = true }
            };
            context.TipoDocumento.AddRange(tiposDocumento);

            // Seed TipoHabitacion
            var tiposHabitacion = new List<TipoHabitacion>
            {
                new TipoHabitacion
                {
                    Nombre = "Estándar",
                    Descripcion = "Ideal para 2 personas, simple.",
                    PrecioBase = 45.00m,
                    CantidadDisponible = 10,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    Servicios = new List<Servicio> { servicios[0], servicios[2] } // WiFi y Estacionamiento
                },
                new TipoHabitacion
                {
                    Nombre = "Deluxe",
                    Descripcion = "Cómoda para dos personas. Cama matrimonial o dos camas.",
                    PrecioBase = 75.00m,
                    CantidadDisponible = 8,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    Servicios = new List<Servicio> { servicios[0], servicios[1], servicios[2] } // Todos
                }
            };
            context.TipoHabitacion.AddRange(tiposHabitacion);

            // Crear Pensiones
            var pensiones = new Pension[]
            {
                new Pension { Nombre = "Sin Pensión", Descripcion = "No incluye comidas", PrecioAdicional = 0, Creacion=now, Actualizacion=now, Activo = true },
                new Pension { Nombre = "Desayuno", Descripcion = "Incluye desayuno", PrecioAdicional = 10, Creacion=now, Actualizacion=now, Activo = true  },
                new Pension { Nombre = "Media Pensión", Descripcion = "Incluye desayuno y cena", PrecioAdicional = 30, Creacion=now, Actualizacion=now, Activo = true  },
                new Pension { Nombre = "Pensión Completa", Descripcion = "Incluye desayuno, almuerzo y cena", PrecioAdicional = 50,Creacion=now, Actualizacion=now, Activo = true  }
            };
            context.Pension.AddRange(pensiones);
            context.SaveChanges();

            // Crear Estados de Reserva
            var estadosReserva = new EstadoReserva[]
            {
                new EstadoReserva {Nombre = "Pendiente", Creacion = now, Actualizacion = now, Activo = true},
                new EstadoReserva {Nombre = "Confirmada", Creacion = now, Actualizacion = now, Activo = true},
                new EstadoReserva {Nombre = "Cancelada", Creacion = now, Actualizacion = now, Activo = true},
                new EstadoReserva {Nombre = "Check-In", Creacion = now, Actualizacion = now, Activo = true},
                new EstadoReserva {Nombre = "Check-Out", Creacion = now, Actualizacion = now, Activo = true},
            };
            context.EstadoReserva.AddRange(estadosReserva);
            context.SaveChanges();

            // Seed Clientes
            var clientes = new List<Cliente>
            {
                new Cliente
                {
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Email = "juan.perez@example.com",
                    Telefono = "123456789",
                    NumDocumento = "12345678A",
                    TipoDocumentoId = tiposDocumento[0].Id,
                    Nacionalidad = "Española",
                    Comentarios = "Cliente VIP",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                },
                new Cliente
                {
                    Nombre = "María",
                    Apellido = "Gómez",
                    Email = "maria.gomez@example.com",
                    Telefono = "987654321",
                    NumDocumento = "98765432B",
                    TipoDocumentoId = tiposDocumento[1].Id,
                    Nacionalidad = "Mexicana",
                    Comentarios = "Cliente frecuente",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                }
            };
            context.Cliente.AddRange(clientes);
            context.SaveChanges();
            //Seed EstadoHabitacion

            var estadoHabitaciones = new List<EstadoHabitacion>
            {
                new EstadoHabitacion {Nombre = "DISPONIBLE", Creacion = now, Actualizacion = now, Activo = true },
                new EstadoHabitacion {Nombre = "OCUPADO", Creacion = now, Actualizacion = now, Activo = true },
                new EstadoHabitacion {Nombre = "EN LIMPIEZA", Creacion = now, Actualizacion = now, Activo = true},
                new EstadoHabitacion {Nombre = "LATE-CHECKOUT", Creacion = now, Actualizacion = now, Activo = true}
            };
            context.EstadoHabitacion.AddRange(estadoHabitaciones);
            context.SaveChanges();
            // Seed Habitaciones
            var habitaciones = new List<Habitacion>
            {
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id,
                    NumeroHabitacion = 101,
                    EstadoHabitacionId = 1,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id,
                    NumeroHabitacion = 102,
                    EstadoHabitacionId = 1,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[1].Id,
                    NumeroHabitacion = 201,
                    EstadoHabitacionId = 1,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.Habitacion.AddRange(habitaciones);
            context.SaveChanges();

            // Seed Reservas
            var reservas = new List<Reserva>
            {
                new Reserva
                {
                    ClienteId = clientes[0].Id,
                    Codigo = "RES001",
                    FechaIngreso = now.AddDays(7),
                    FechaSalida = now.AddDays(10),
                    LlegadaEstimada = new TimeOnly(14, 0),
                    Comentarios = "Llegan en la tarde",
                    EstadoId = estadosReserva[0].Id, // Pendiente
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                },
                 new Reserva
                {
                    ClienteId = clientes[1].Id,
                    Codigo = "RES002",
                    FechaIngreso = now.AddDays(10),
                    FechaSalida = now.AddDays(15),
                    LlegadaEstimada = new TimeOnly(12, 0),
                    Comentarios = "Necesitan cuna",
                    EstadoId = estadosReserva[1].Id, // Confirmada
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                }
            };
            context.Reserva.AddRange(reservas);
            context.SaveChanges();

            // Seed DetalleReserva
            var detallesReserva = new List<DetalleReserva>
            {
                new DetalleReserva
                {
                    ReservaId = reservas[0].Id,
                    HabitacionId = habitaciones[0].Id,
                    CantidadAdultos = 2,
                    CantidadNinhos = 1,
                    PensionId = pensiones[1].Id, // Desayuno
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new DetalleReserva
                {
                    ReservaId = reservas[1].Id,
                    HabitacionId = habitaciones[2].Id,
                    CantidadAdultos = 2,
                    CantidadNinhos = 0,
                    PensionId = pensiones[2].Id, // Media Pension
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.DetalleReserva.AddRange(detallesReserva);
            context.SaveChanges();

            // Seed Checkin
            var checkins = new List<Checkin>
            {
                new Checkin
                {
                    ReservaId = reservas[0].Id,
                    FechaCheckIn = now.AddDays(7),
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    DetalleHuespedes = new Collection<DetalleHuesped>
                    {
                        new DetalleHuesped{ Nombre = "Huesped 1", NumDocumento="Doc1", Creacion=now, Actualizacion=now, Activo=true},
                        new DetalleHuesped{ Nombre = "Huesped 2", NumDocumento="Doc2", Creacion=now, Actualizacion=now, Activo=true}
                    }
                }
            };
            context.Checkin.AddRange(checkins);
            context.SaveChanges();

            // Seed Checkout
            var checkouts = new List<Checkout>
            {
                new Checkout
                {
                    ReservaId = reservas[0].Id,
                    FechaCheckOut = now.AddDays(10),
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.Checkout.AddRange(checkouts);
            context.SaveChanges();

            // Seed Cancelacion
            var cancelaciones = new List<Cancelacion>
            {
                new Cancelacion
                {
                    DetalleReservaId = detallesReserva[0].Id,
                    Motivo = "Cambio de planes",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.Cancelacion.AddRange(cancelaciones);
            context.SaveChanges();

            // Seed Consultas
            var consultas = new List<Consulta>
            {
                new Consulta
                {
                    Nombre = "Consulta 1",
                    Email = "consulta1@example.com",
                    Telefono = "555-1234",
                    Mensaje = "Mensaje de prueba 1",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Consulta
                {
                    Nombre = "Consulta 2",
                    Email = "consulta2@example.com",
                    Telefono = "555-5678",
                    Mensaje = "Mensaje de prueba 2",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.Consulta.AddRange(consultas);
            context.SaveChanges();

            // Seed Solicitudes
            var solicitudes = new List<Solicitud>
            {
                new Solicitud
                {
                    ReservaId = reservas[0].Id,
                    CancelacionId = cancelaciones[0].Id,
                    ConsultaId = null,
                    EsLeida = false,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Solicitud
                {
                    ReservaId = null,
                    CancelacionId = null,
                    ConsultaId = consultas[0].Id,
                    EsLeida = false,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
            context.Solicitud.AddRange(solicitudes);
            context.SaveChanges();

            // Seed Usuarios
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Nombre = "Manuel Ayala",
                    Username = "mayala",
                    HashContrasenha = "AASDUYDGFAS7YF112323",
                    UltimoLogin = now,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Otto Enzler",
                    Username = "oenzler",
                    HashContrasenha = "AA{}{asdñ+123123asd",
                    UltimoLogin = now,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Tania Schulz",
                    Username = "tschulz",
                    HashContrasenha = "KGJSIDUF*´{*][ASDASD",
                    UltimoLogin = now,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };
        }
    }
}
