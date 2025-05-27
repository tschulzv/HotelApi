using System;
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

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckinsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public CheckinsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Checkins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckinDTO>>> GetCheckin()
        {
            var checkins = await _context.Checkin
            .Where(c => c.Activo)
            .Include(c => c.DetalleHuespedes)
            .ToListAsync();
            var checkInsDTO = checkins.Select(c => ToDTO(c)).ToList();
            return Ok(checkInsDTO);
        }

        // GET: api/Checkins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckinDTO>> GetCheckin(int id)
        {
            var checkin = await _context.Checkin
            .Where(c => c.Activo)
            .Include(c => c.DetalleHuespedes)
            .FirstOrDefaultAsync(c => c.Id == id);

            if (checkin == null)
            {
                return NotFound();
            }

            return ToDTO(checkin);
        }

        // PUT: api/Checkins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckin(int id, CheckinDTO ciDto)
        {
            if (id != ciDto.Id)
            {
                return BadRequest();
            }

            var checkin = await _context.Checkin
                .Include(c => c.DetalleHuespedes)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (checkin == null)
            {
                return NotFound();
            }

            checkin.ReservaId = ciDto.ReservaId;
            
            var existingDetalleHuespedes = checkin.DetalleHuespedes.ToList();
            var updatedDetalleHuespedes = ciDto.DetalleHuespedes.Select(dto => new DetalleHuesped
            {
                Id = dto.Id, // Si el DTO incluye el ID del DetalleHuesped existente
                Nombre = dto.Nombre,
                NumDocumento = dto.NumDocumento
            }).ToList();

            // Eliminar los DetalleHuespedes que ya no están en el DTO
            foreach (var existingHuesped in existingDetalleHuespedes)
            {
                if (!updatedDetalleHuespedes.Any(updated => updated.Id == existingHuesped.Id && updated.Id != 0)) // Considerar ID 0 para nuevos
                {
                    _context.DetalleHuesped.Remove(existingHuesped);
                }

            }
            // Agregar o actualizar los DetalleHuespedes del DTO
            foreach (var updatedHuesped in updatedDetalleHuespedes)
            {
                var existing = existingDetalleHuespedes.FirstOrDefault(e => e.Id == updatedHuesped.Id && e.Id != 0);
                if (existing != null)
                {
                    _context.Entry(existing).CurrentValues.SetValues(updatedHuesped);
                }
                else
                {
                    checkin.DetalleHuespedes.Add(updatedHuesped);
                }
            }

            checkin.Actualizacion = DateTime.Now;
            _context.Entry(checkin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckinExists(id))
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

        // POST: api/Checkins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CheckinDTO>> PostCheckin(CheckinDTO checkinDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- IDs de Estado (Actualizado) ---
            const int ID_ESTADO_RESERVA_CHECKIN = 4;
            const int ID_ESTADO_HABITACION_OCUPADO = 2; // <--- ¡CAMBIO IMPORTANTE AQUÍ!

            // 1. Recuperar la Reserva y sus detalles (incluyendo las habitaciones)
            var reserva = await _context.Reserva
                .Include(r => r.Detalles)
                    .ThenInclude(dr => dr.Habitacion)
                .FirstOrDefaultAsync(r => r.Id == checkinDTO.ReservaId && r.Activo);

            if (reserva == null)
            {
                return NotFound(new { Mensaje = $"Reserva con ID {checkinDTO.ReservaId} no encontrada o no está activa." });
            }

            // Validar si la reserva ya está en Check-In o un estado posterior (Check-Out, Cancelada)
            if (reserva.EstadoId == ID_ESTADO_RESERVA_CHECKIN)
            {
                return BadRequest(new { Mensaje = "La reserva ya se encuentra en estado Check-In." });
            }
            if (reserva.EstadoId == 5 /* Check-Out */ || reserva.EstadoId == 3 /* Cancelada */)
            {
                return BadRequest(new { Mensaje = "No se puede hacer Check-In a una reserva que ya está en Check-Out o Cancelada." });
            }
            // (Otras validaciones de estado de reserva si son necesarias)


            // 2. Actualizar el Estado de la Reserva
            reserva.EstadoId = ID_ESTADO_RESERVA_CHECKIN;
            reserva.Actualizacion = DateTime.UtcNow;
            _context.Entry(reserva).State = EntityState.Modified;

            // 3. Actualizar el Estado de cada Habitación asociada a la Reserva
            if (reserva.Detalles != null && reserva.Detalles.Any())
            {
                foreach (var detalleReserva in reserva.Detalles)
                {
                    if (detalleReserva.Habitacion != null)
                    {
                        // Validar si la habitación ya está ocupada (ahora con el ID correcto)
                        if (detalleReserva.Habitacion.EstadoHabitacionId == ID_ESTADO_HABITACION_OCUPADO && detalleReserva.Habitacion.Activo)
                        {
                            Console.WriteLine($"Advertencia: La habitación ID {detalleReserva.Habitacion.Id} ya estaba marcada como ocupada (Estado ID: {ID_ESTADO_HABITACION_OCUPADO}).");
                        }
                        detalleReserva.Habitacion.EstadoHabitacionId = ID_ESTADO_HABITACION_OCUPADO; // Usa el ID correcto
                        detalleReserva.Habitacion.Actualizacion = DateTime.UtcNow;
                        _context.Entry(detalleReserva.Habitacion).State = EntityState.Modified;
                    }
                    else
                    {
                        Console.WriteLine($"Error: No se pudo cargar la habitación para DetalleReserva ID {detalleReserva.Id}");
                    }
                }
            }
            else
            {
                return BadRequest(new { Mensaje = "La reserva no tiene detalles de habitación asignados." });
            }

            // 4. Crear el registro de Checkin
            var checkin = new Checkin
            {
                ReservaId = checkinDTO.ReservaId,
                Activo = true,
                DetalleHuespedes = new System.Collections.ObjectModel.Collection<DetalleHuesped>(
                    checkinDTO.DetalleHuespedes?.Select(dto => new DetalleHuesped
                    {
                        Nombre = dto.Nombre,
                        NumDocumento = dto.NumDocumento,
                        Activo = true,
                    }).ToList() ?? new List<DetalleHuesped>()
                ),
                Creacion = DateTime.UtcNow,
                Actualizacion = DateTime.UtcNow
            };

            _context.Checkin.Add(checkin);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Mensaje = "Error al guardar los datos del check-in.", Detalle = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Ocurrió un error inesperado durante el check-in.", Detalle = ex.Message });
            }

            return CreatedAtAction(nameof(GetCheckin), new { id = checkin.Id }, ToDTO(checkin));
        }


        // DELETE: api/Checkins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckin(int id)
        {
            var checkin = await _context.Checkin.FindAsync(id);
            if (checkin == null || !checkin.Activo)
            {
                return NotFound();
            }

            checkin.Activo = false;
            checkin.Actualizacion = DateTime.Now;

            _context.Entry(checkin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckinExists(id))
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

        private bool CheckinExists(int id)
        {
            return _context.Checkin.Any(e => e.Id == id);
        }

        private static CheckinDTO ToDTO(Checkin ci)
        {
            return new CheckinDTO
            {
                Id = ci.Id,
                ReservaId = ci.ReservaId,
                DetalleHuespedes = ci.DetalleHuespedes.Select(dh => new DetalleHuespedDTO
                {
                    Id = dh.Id,
                    Nombre = dh.Nombre,
                    NumDocumento = dh.NumDocumento
                }).ToList() ?? new List<DetalleHuespedDTO>()
            };
        }
    }
}
