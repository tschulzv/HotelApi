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
            var cancelaciones = await _context.Cancelacion.Where(c => c.Activo).ToListAsync();
            var cancelacionDTOs = cancelaciones.Select(ca => ToDTO(ca));
            return Ok(cancelacionDTOs);
        }

        // GET: api/Cancelacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CancelacionDTO>> GetCancelacion(int id)
        {
            var cancelacion = await _context.Cancelacion.Where(c => c.Activo && c.Id == id).FirstOrDefaultAsync();

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
            ca.DetalleReservaId = caDto.DetalleReservaId;
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

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Crear la cancelación
                var cancelacion = new Cancelacion
                {
                    DetalleReservaId = caDto.DetalleReservaId,
                    ReservaId = caDto.ReservaId,
                    Motivo = caDto.Motivo,
                    Creacion = DateTime.Now,
                    Actualizacion = DateTime.Now,
                    Activo = true
                };

                _context.Cancelacion.Add(cancelacion);

                // si el detalle no es null, se desea cancelar solo un detalle
                if (caDto.DetalleReservaId != null)
                {
                    // Inactivar el detalle de reserva
                    var detalle = await _context.DetalleReserva.FindAsync(caDto.DetalleReservaId);
                    if (detalle == null)
                        return NotFound($"No se encontró el DetalleReserva con ID {caDto.DetalleReservaId}");

                    detalle.Activo = false;
                    detalle.Actualizacion = DateTime.Now;
                } else if (caDto.ReservaId != null)
                {
                    // cambiar el estado de la reserva a cancelada
                    var res = await _context.Reserva.FindAsync(caDto.ReservaId);
                    if (res == null)
                        return NotFound($"No se encontró Reserva con ID {caDto.ReservaId}");

                    res.EstadoId = 3;
                    res.Actualizacion = DateTime.Now;
                }

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
                DetalleReservaId = ca.DetalleReservaId,
                Motivo = ca.Motivo,
                Activo = ca.Activo
            };
        }
    }
}
