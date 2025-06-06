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
    public class CancelacionsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public CancelacionsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Cancelacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CancelacionDTO>>> GetCancelacion()
        {
            var cancelaciones = await _context.Cancelacion
                .Where(c => c.Activo)
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente) //agregue esto
                .ToListAsync();
            var cancelacionDTOs = cancelaciones.Select(ca => ToDTO(ca));
            return Ok(cancelacionDTOs);
        }

        // GET: api/Cancelacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CancelacionDTO>> GetCancelacion(int id)
        {
            var cancelacion = await _context.Cancelacion
                .Where(c => c.Activo && c.Id == id)
                .Include(c => c.Reserva)
                    .ThenInclude(r => r.Cliente) // agregue esto
                .FirstOrDefaultAsync();

            if (cancelacion == null)
            {
                return NotFound();
            }

            return ToDTO(cancelacion);
        }

        // PUT: api/Cancelacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCancelacion(int id, CancelacionDTO caDto)
        {
            if (id != caDto.Id)
            {
                return BadRequest();
            }

            var ca = await _context.Cancelacion.FindAsync(id);

            if(ca == null)
            {
                return NotFound();
            }
            ca.Id = caDto.Id;
            ca.Motivo = caDto.Motivo;
            ca.Activo = caDto.Activo;
            ca.Actualizacion = DateTime.Now;

            _context.Entry(ca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CancelacionExists(id))
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

        // POST: api/Cancelacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cancelacion>> PostCancelacion(CancelacionDTO caDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (caDto.ReservaId == null)
                return BadRequest("Debe proporcionar un ReservaId.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var reserva = await _context.Reserva
                    .Include(r => r.Detalles)
                    .FirstOrDefaultAsync(r => r.Id == caDto.ReservaId);

                if (reserva == null)
                    return NotFound($"No se encontró Reserva con ID {caDto.ReservaId}");

                var cancelacion = new Cancelacion
                {
                    ReservaId = caDto.ReservaId.Value,
                    Motivo = caDto.Motivo,
                    Creacion = DateTime.Now,
                    Actualizacion = DateTime.Now,
                    Activo = true,
                    DetalleReservaIds = new List<int>()
                };

                if (caDto.DetalleReservaIds != null && caDto.DetalleReservaIds.Any())
                {
                    foreach (var detalleId in caDto.DetalleReservaIds)
                    {
                        var detalle = await _context.DetalleReserva.FindAsync(detalleId);

                        if (detalle == null)
                            return NotFound($"No se encontró DetalleReserva con ID {detalleId}");

                        if (detalle.ReservaId != caDto.ReservaId)
                            return BadRequest($"El DetalleReserva ID {detalleId} no pertenece a la reserva ID {caDto.ReservaId}");

                        if (!detalle.Activo)
                            return BadRequest($"El DetalleReserva ID {detalleId} ya está cancelado.");

                        detalle.Activo = false;
                        detalle.Actualizacion = DateTime.Now;

                        cancelacion.DetalleReservaIds.Add(detalleId);
                    }
                }
                else
                {
                    // Cancelación total de la reserva
                    reserva.EstadoId = 3; // Asumimos que EstadoId 3 = Cancelada
                    reserva.Actualizacion = DateTime.Now;
                }

                _context.Cancelacion.Add(cancelacion);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction("GetCancelacion", new { id = cancelacion.Id }, ToDTO(cancelacion));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error al cancelar: {ex.Message}");
            }
        }



        // DELETE: api/Cancelacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancelacion(int id)
        {
            var cancelacion = await _context.Cancelacion.FindAsync(id);
            if (cancelacion == null || !cancelacion.Activo)
            {
                return NotFound();
            }

            cancelacion.Activo = false;
            cancelacion.Actualizacion = DateTime.Now;

            _context.Entry(cancelacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CancelacionExists(id))
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

        private bool CancelacionExists(int id)
        {
            return _context.Cancelacion.Any(e => e.Id == id);
        }

        public static CancelacionDTO ToDTO(Cancelacion ca)
        {
            return new CancelacionDTO
            {
                Id = ca.Id,
                ReservaId = ca.ReservaId,
                Reserva = ca.ReservaId != null ? ReservasController.ToDTO(ca.Reserva) : null,
                Motivo = ca.Motivo,
                Activo = ca.Activo,
                DetalleReservaIds = ca.DetalleReservaIds
            };
        }
    }
}
