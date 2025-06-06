﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;
using Microsoft.AspNetCore.Authorization;
using HotelApi.DTOs;
using HotelApi.DTOs.Request;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public HabitacionsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Habitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitacionDTO>>> GetHabitacion()
        {
            var habitaciones = await _context.Habitacion
            .Include(h => h.EstadoHabitacion)
            .Include(h => h.TipoHabitacion)
            .Where(h=> h.Activo ==true)
            .Select(h => new HabitacionDTO
            {
                Id = h.Id,
                NumeroHabitacion = h.NumeroHabitacion,
                TipoHabitacionId = h.TipoHabitacionId,
                TipoHabitacionNombre = h.TipoHabitacion.Nombre,
                EstadoHabitacionId = h.EstadoHabitacionId,
                EstadoNombre = h.EstadoHabitacion.Nombre,
                Observaciones = h.Observaciones,
                Activo = h.Activo
            })
            .ToListAsync();
            return Ok(habitaciones);
        }

        // GET: api/Habitacions/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HabitacionDTO>> GetHabitacion(int id)
        {
            var habitacion = await _context.Habitacion
            .Include(h => h.EstadoHabitacion)
            .Include(h => h.TipoHabitacion)
            .Where(h => h.Id == id && h.Activo == true)
            .Select(h => new HabitacionDTO
            {
                Id = h.Id,
                NumeroHabitacion = h.NumeroHabitacion,
                EstadoHabitacionId = h.EstadoHabitacionId,
                EstadoNombre = h.EstadoHabitacion.Nombre,
                TipoHabitacionId = h.TipoHabitacionId,
                TipoHabitacionNombre = h.TipoHabitacion.Nombre,
                Observaciones = h.Observaciones,
                Activo = h.Activo
            })
            .FirstOrDefaultAsync();

            if (habitacion == null)
            {
                return NotFound();
            }

            return habitacion;
        }


        [HttpPost("disponibles")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitacionesDisponibles([FromBody] DisponibilidadRequest request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Recibiendo solicitud de disponibilidad:");
            Console.WriteLine($"  CheckIn: {request.CheckIn}");
            Console.WriteLine($"  CheckOut: {request.CheckOut}");
            Console.WriteLine("  Habitaciones Solicitadas:");
            if (request.HabitacionesSolicitadas != null && request.HabitacionesSolicitadas.Any())
            {
                foreach (var roomReq in request.HabitacionesSolicitadas)
                {
                    Console.WriteLine($"    Adultos: {roomReq.Adultos}, Niños: {roomReq.Ninos}, Tipo: {roomReq.TipoHabitacionId?.ToString() ?? "Cualquiera"}");
                }
            }
            else
            {
                Console.WriteLine("    Ninguna especificada.");
            }
            Console.WriteLine("--------------------------------------------------");

            var estadoActivos = new[] { 1, 2 };

            var habitacionesOcupadas = await _context.DetalleReserva
                .Include(dr => dr.Reserva)
                .Where(dr =>
                    estadoActivos.Contains(dr.Reserva.EstadoId) &&
                    dr.Reserva.FechaIngreso < request.CheckOut &&
                    dr.Reserva.FechaSalida > request.CheckIn &&
                    dr.Activo)
                .Select(dr => dr.HabitacionId)
                .Distinct()
                .ToListAsync();

            var habitacionesDisponibles = await _context.Habitacion
                .Include(h => h.TipoHabitacion)
                    .ThenInclude(th => th.Servicios)
                .Include(h => h.TipoHabitacion)
                    .ThenInclude(th => th.ImagenesHabitaciones)
                .Where(h =>
                    !habitacionesOcupadas.Contains(h.Id) &&
                    h.Activo)
                .ToListAsync();

            // Filtrado individual por RoomRequest (tipo + capacidad)
            var habitacionesFiltradas = new List<Habitacion>();
            Console.WriteLine("Filtrando habitaciones por capacidad y tipo:");

            foreach (var roomReq in request.HabitacionesSolicitadas)
            {
                var capacidadRequerida = roomReq.Adultos + roomReq.Ninos;
                Console.WriteLine($"  Requiere: {roomReq.Adultos} adultos, {roomReq.Ninos} niños (capacidad {capacidadRequerida}), Tipo ID: {roomReq.TipoHabitacionId?.ToString() ?? "Cualquiera"}");

                var habitacionesCumplen = habitacionesDisponibles.Where(h =>
                    h.TipoHabitacion.MaximaOcupacion >= capacidadRequerida &&
                    (!roomReq.TipoHabitacionId.HasValue || h.TipoHabitacionId == roomReq.TipoHabitacionId.Value)
                ).ToList();

                if (habitacionesCumplen.Any())
                {
                    habitacionesFiltradas.AddRange(habitacionesCumplen);
                    foreach (var hab in habitacionesCumplen)
                    {
                        Console.WriteLine($"    OK -> ID: {hab.Id}, Número: {hab.NumeroHabitacion}, Tipo: {hab.TipoHabitacion?.Nombre}, Capacidad: {hab.TipoHabitacion?.MaximaOcupacion}");
                    }
                }
                else
                {
                    Console.WriteLine("    ❌ No se encontraron habitaciones con esos criterios.");
                }
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Resumen final de habitaciones filtradas:");
            foreach (var h in habitacionesFiltradas)
            {
                Console.WriteLine($"  ID: {h.Id}, Tipo: {h.TipoHabitacion?.Nombre}");
            }
            Console.WriteLine("--------------------------------------------------");

            if (request.isRequestRoomData)
            {
                var h = habitacionesFiltradas.Select(h => ToDTO(h));
                return Ok(h);
            }
            // Agrupar para construir DTOs
            var resultado = habitacionesFiltradas
                .GroupBy(h => h.TipoHabitacion)
                .Select(g => new TipoHabitacionDTO
                {
                    Id = g.Key.Id,
                    Nombre = g.Key.Nombre,
                    Descripcion = g.Key.Descripcion,
                    PrecioBase = g.Key.PrecioBase,
                    CantidadDisponible = g.Count(),
                    MaximaOcupacion = g.Key.MaximaOcupacion,
                    Tamanho = g.Key.Tamanho,
                    Servicios = g.Key.Servicios?.Select(s => new ServicioDTO
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                        IconName = s.IconName
                    }).ToList() ?? new List<ServicioDTO>(),
                    Imagenes = g.Key.ImagenesHabitaciones?.Select(i => new ImagenHabitacionDTO
                    {
                        Id = i.Id,
                        Url = $"{baseUrl}/imagenes/{i.Url}"
                    }).ToList() ?? new List<ImagenHabitacionDTO>()
                })
                .ToList();

            return Ok(resultado);
        }



        // PUT: api/Habitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHabitacion(int id, HabitacionDTO habitacionDTO)
        {
            if (id != habitacionDTO.Id)
            {
                return BadRequest();
            }

            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            habitacion.TipoHabitacionId = habitacionDTO.TipoHabitacionId;
            habitacion.NumeroHabitacion = habitacionDTO.NumeroHabitacion;
            habitacion.EstadoHabitacionId= habitacionDTO.EstadoHabitacionId;
            habitacion.Observaciones = habitacionDTO.Observaciones;
            habitacion.Activo = habitacionDTO.Activo;
            habitacion.Actualizacion = DateTime.Now; // Actualizar la fecha de actualización
            _context.Entry(habitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Habitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HabitacionDTO>> PostHabitacion(HabitacionDTO habitacionDTO)
        {
            var habitacion = new Habitacion
            {
                TipoHabitacionId = habitacionDTO.TipoHabitacionId,
                NumeroHabitacion = habitacionDTO.NumeroHabitacion,
                EstadoHabitacionId = habitacionDTO.EstadoHabitacionId,
                Observaciones = habitacionDTO.Observaciones,
                Activo = habitacionDTO.Activo,
                Creacion = DateTime.Now,       // Establecer la fecha de creación
                Actualizacion = DateTime.Now    // Establecer la fecha de actualización
            };

            _context.Habitacion.Add(habitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHabitacion), new { id = habitacion.Id }, ToDTO(habitacion));
        }

        // DELETE: api/Habitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            var habitacion = await _context.Habitacion
                .Include(h => h.DetalleReservas) // Si tienes esta relación
                .FirstOrDefaultAsync(h => h.Id == id);

            if (habitacion == null || !habitacion.Activo)
            {
                return NotFound();
            }

            // Validar si hay reservas activas asociadas
            if (habitacion.DetalleReservas != null && habitacion.DetalleReservas.Any(dr => dr.Activo))
            {
                return BadRequest("No se puede eliminar la habitación porque tiene reservas activas asociadas.");
            }

            habitacion.Activo = false;
            habitacion.Actualizacion = DateTime.Now;

            _context.Entry(habitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitacion.Any(e => e.Id == id);
        }

        public static HabitacionDTO ToDTO(Habitacion habitacion)
        {
            return new HabitacionDTO
            {
                Id = habitacion.Id,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                EstadoHabitacionId = habitacion.EstadoHabitacionId,
                Observaciones = habitacion.Observaciones,
                Activo = habitacion.Activo
            };
        }
    }
}
