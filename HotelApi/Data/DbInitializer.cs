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
                new Servicio { Nombre = "Minibar", IconName = "local_bar", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Balcón", IconName = "deck", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Caja fuerte", IconName = "lock", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Secador de pelo", IconName = "hair_dryer", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Servicio de lavandería", IconName = "local_laundry_service", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Vista al mar", IconName = "beach_access", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Acceso al spa", IconName = "self_care", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Jacuzzi", IconName = "bathtub", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Wi-Fi", IconName = "wifi", Creacion = now, Actualizacion = now, Activo = true },
                // Servicios adicionales
                new Servicio { Nombre = "Aire acondicionado", IconName = "ac_unit", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Calefacción", IconName = "thermostat", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Recepción 24 horas", IconName = "room_service", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Gimnasio", IconName = "fitness_center", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Traslado al aeropuerto", IconName = "airport_shuttle", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Adaptado para movilidad reducida", IconName = "accessible", Creacion = now, Actualizacion = now, Activo = true },
                new Servicio { Nombre = "Terraza / Jardín", IconName = "yard", Creacion = now, Actualizacion = now, Activo = true }
            };

            context.Servicio.AddRange(servicios);

            // Seed TipoDocumento
            var tiposDocumento = new List<TipoDocumento>
            {
                new TipoDocumento { Nombre = "Cédula de Identidad Paraguaya", Creacion = now, Actualizacion = now, Activo = true },
                new TipoDocumento { Nombre = "Pasaporte", Creacion = now, Actualizacion = now, Activo = true },
                new TipoDocumento { Nombre = "Documento Extranjero", Creacion = now, Actualizacion = now, Activo = true }
            };
            context.TipoDocumento.AddRange(tiposDocumento);

            // Seed TipoHabitacion
            var tiposHabitacion = new List<TipoHabitacion>
            {
                new TipoHabitacion
                {
                    // El 'Id' del JSON no se incluye directamente aquí si es auto-generado por la base de datos.
                    Nombre = "Estándar",
                    Descripcion = "Nuestra Habitación Estándar es la opción ideal para viajeros que buscan comodidad y funcionalidad a un precio accesible. Equipada con todo lo esencial, ofrece un espacio acogedor y práctico para garantizar un descanso reparador. Disfruta de una cama cómoda, baño privado y las comodidades básicas para una estancia placentera. Es perfecta para quienes priorizan la eficiencia sin sacrificar el confort.",
                    PrecioBase = 40.00m, // Usa 'm' para indicar que es un decimal
                    CantidadDisponible = 10,
                    MaximaOcupacion = 2,
                    Tamanho = 30,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    ImagenesHabitaciones = new List<ImagenHabitacion>
                    {
                        new ImagenHabitacion { Url = "d94dfde4-e66b-49d9-a95d-24b224a12a5e.jpg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "3c6b1771-e541-4b95-8536-36636f057253.jpg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "dd4b9aec-51eb-47af-a69a-f97f46cc3e74.png", Creacion = now, Actualizacion = now, Activo = true }
                    }
                    // No se incluyen servicios aquí ya que tu JSON de entrada no los contenía.
                    // Si tienes servicios, necesitarías definirlos primero y luego referenciarlos aquí.
                    // Ejemplo: Servicios = new List<Servicio> { servicios[0], servicios[2] }
                },
                new TipoHabitacion
                {
                    // Id = 2,
                    Nombre = "Deluxe",
                    Descripcion = "La Habitación Deluxe eleva tu experiencia de hospedaje. Más espaciosa y con un diseño elegante, esta habitación te ofrece un ambiente sofisticado y relajante. Disfruta de acabados de mayor calidad, mobiliario confortable y servicios adicionales pensados para tu bienestar. Es la elección perfecta para aquellos que desean un toque extra de lujo y amplitud durante su visita.",
                    PrecioBase = 30.00m,
                    CantidadDisponible = 5,
                    MaximaOcupacion = 3,
                    Tamanho = 40,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    ImagenesHabitaciones = new List<ImagenHabitacion>
                    {
                        new ImagenHabitacion { Url = "d7334f63-0091-43ff-9599-b630ac628c62.jpeg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "d7c7c54e-0159-46c1-8ffc-650fdb4a5ee4.jpeg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "eb4e0082-46d0-426e-a645-5aeaf2c1e0e5.jpeg", Creacion = now, Actualizacion = now, Activo = true }
                    }
                    // Servicios no incluidos.
                },
                new TipoHabitacion
                {
                    // Id = 3,
                    Nombre = "Ejecutiva",
                    Descripcion = "Diseñada pensando en el viajero de negocios, la Habitación Ejecutiva combina confort y funcionalidad para optimizar tu productividad. Además de un espacio elegante, cuenta con un amplio escritorio de trabajo, acceso a internet de alta velocidad y servicios adaptados a tus necesidades profesionales. Ofrece un entorno tranquilo y eficiente para que puedas trabajar y relajarte sin interrupciones.",
                    PrecioBase = 60.00m,
                    CantidadDisponible = 4,
                    MaximaOcupacion = 4,
                    Tamanho = 60,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    ImagenesHabitaciones = new List<ImagenHabitacion>
                    {
                        new ImagenHabitacion { Url = "dc1cf618-857d-4222-8e8f-cb27ad9233d1.png", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "502d1b20-7f27-4130-a9e3-5c536bae365a.png", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "8911fc9d-229d-45d7-877c-25afbb010999.png", Creacion = now, Actualizacion = now, Activo = true }
                    }
                    // Servicios no incluidos.
                },
                new TipoHabitacion
                {
                    // Id = 4,
                    Nombre = "Presidencial",
                    Descripcion = "Experimenta la máxima expresión de lujo y exclusividad en nuestra Habitación Presidencial. Este espacio sublime ofrece una experiencia inigualable con un diseño grandioso, áreas de estar y dormitorio separadas, y vistas panorámicas. Equipada con las más finas amenidades y un servicio personalizado, es ideal para quienes buscan privacidad, opulencia y un nivel de confort sin precedentes. Perfecta para ocasiones especiales o estancias donde el lujo es la prioridad.",
                    PrecioBase = 100.00m,
                    CantidadDisponible = 4,
                    MaximaOcupacion = 5,
                    Tamanho = 100,
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true,
                    ImagenesHabitaciones = new List<ImagenHabitacion>
                    {
                        new ImagenHabitacion { Url = "c460bc63-4670-4c84-a561-e00945e9dacc.jpg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "501fbf5d-d302-424b-a316-c28b3f890458.jpg", Creacion = now, Actualizacion = now, Activo = true },
                        new ImagenHabitacion { Url = "4f2e70e8-7986-4bdf-bf94-94cd11a47452.png", Creacion = now, Actualizacion = now, Activo = true }
                    }
                    // Servicios no incluidos.
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
            /*
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
            */
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
            // Asegúrate de que 'now' sea una variable DateTime ya definida en tu contexto, por ejemplo:
            // DateTime now = DateTime.UtcNow; // O DateTime.Now;

            var habitaciones = new List<Habitacion>
{
                // Habitaciones Estándar (usando tiposHabitacion[0].Id)
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id, // Estándar
                    NumeroHabitacion = 101,
                    EstadoHabitacionId = 1, // Asume un estado inicial como "Disponible"
                    Observaciones = "Cerca del ascensor. Vista interior.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id, // Estándar
                    NumeroHabitacion = 102,
                    EstadoHabitacionId = 1,
                    Observaciones = "Cama doble. Silenciosa.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id, // Estándar
                    NumeroHabitacion = 103,
                    EstadoHabitacionId = 1,
                    Observaciones = "Dos camas individuales.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[0].Id, // Estándar
                    NumeroHabitacion = 104,
                    EstadoHabitacionId = 1,
                    Observaciones = "Con vistas a la ciudad. Balcón pequeño.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },

                // Habitaciones Deluxe (usando tiposHabitacion[1].Id)
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[1].Id, // Deluxe
                    NumeroHabitacion = 201,
                    EstadoHabitacionId = 1,
                    Observaciones = "Amplia con sofá. Vista a la piscina.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[1].Id, // Deluxe
                    NumeroHabitacion = 202,
                    EstadoHabitacionId = 1,
                    Observaciones = "Cama king-size. Baño espacioso.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[1].Id, // Deluxe
                    NumeroHabitacion = 203,
                    EstadoHabitacionId = 1,
                    Observaciones = "Con balcón grande.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },

                // Habitaciones Ejecutivas (usando tiposHabitacion[2].Id)
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[2].Id, // Ejecutiva
                    NumeroHabitacion = 301,
                    EstadoHabitacionId = 1,
                    Observaciones = "Ideal para viajeros de negocios. Escritorio amplio.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[2].Id, // Ejecutiva
                    NumeroHabitacion = 302,
                    EstadoHabitacionId = 1,
                    Observaciones = "Suite con pequeña sala de estar.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                },

                // Habitaciones Presidenciales (usando tiposHabitacion[3].Id)
                new Habitacion
                {
                    TipoHabitacionId = tiposHabitacion[3].Id, // Presidencial
                    NumeroHabitacion = 401,
                    EstadoHabitacionId = 1,
                    Observaciones = "La suite principal. Vistas panorámicas y jacuzzi.",
                    Creacion = now,
                    Actualizacion = now,
                    Activo = true
                }
            };

            context.Habitacion.AddRange(habitaciones);
            context.SaveChanges();
            /*
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
                    CancelacionId = null,
                    ConsultaId = null,
                    EsLeida = false,
                    Tipo = "Reserva",
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
                    Tipo = "Consulta",
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
            */
        }
    }
}
