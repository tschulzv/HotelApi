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

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DetalleHuespedsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public DetalleHuespedsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/DetalleHuespeds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleHuespedDTO>>> GetDetalleHuesped()
        {
            var detalleHuespeds = await _context.DetalleHuesped.Where(d => d.Activo).ToListAsync();

            return detalleHuespeds.Select(ToDTO).ToList();
        }

        // GET: api/DetalleHuespeds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleHuespedDTO>> GetDetalleHuesped(int id)
        {
            
            var detalleHuesped = await _context.DetalleHuesped.Where(d => d.Activo && d.Id == id).FirstOrDefaultAsync();

            if (detalleHuesped == null)
            {
                return NotFound();
            }

            return ToDTO(detalleHuesped);
        }

        // PUT: api/DetalleHuespeds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleHuesped(int id, DetalleHuespedDTO detalleHuespedDTO)
        {
            if (id != detalleHuespedDTO.Id)
            {
                return BadRequest();
            }

            var detalleHuesped = await _context.DetalleHuesped.FindAsync(id);
            if (detalleHuesped == null)
            {
                return NotFound();
            }

            detalleHuesped.CheckInId = detalleHuespedDTO.CheckInId;
            detalleHuesped.Nombre = detalleHuespedDTO.Nombre;
            detalleHuesped.NumDocumento = detalleHuespedDTO.NumDocumento;
            detalleHuesped.Activo = detalleHuespedDTO.Activo;
            detalleHuesped.Actualizacion = DateTime.Now; // Actualizar la fecha de actualización

            _context.Entry(detalleHuesped).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleHuespedExists(id))
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

        // POST: api/DetalleHuespeds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleHuespedDTO>> PostDetalleHuesped(DetalleHuespedDTO detalleHuespedDTO)
        {
            var detalleHuesped = new DetalleHuesped
            {
                CheckInId = detalleHuespedDTO.CheckInId,
                Nombre = detalleHuespedDTO.Nombre,
                NumDocumento = detalleHuespedDTO.NumDocumento,
                Activo = detalleHuespedDTO.Activo,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now
            };
            _context.DetalleHuesped.Add(detalleHuesped);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetalleHuesped), new { id = detalleHuesped.Id }, ToDTO(detalleHuesped));
        }

        // DELETE: api/DetalleHuespeds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleHuesped(int id)
        {
            var detalleHuesped = await _context.DetalleHuesped.FindAsync(id);
            if (detalleHuesped == null || !detalleHuesped.Activo)
            {
                return NotFound();
            }

            detalleHuesped.Activo = false;
            detalleHuesped.Actualizacion = DateTime.Now;

            _context.Entry(detalleHuesped).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleHuespedExists(id))
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

        private bool DetalleHuespedExists(int id)
        {
            return _context.DetalleHuesped.Any(e => e.Id == id);
        }

        private static DetalleHuespedDTO ToDTO(DetalleHuesped detalleHuesped)
        {
            return new DetalleHuespedDTO
            {
                Id = detalleHuesped.Id,
                CheckInId = detalleHuesped.CheckInId,
                Nombre = detalleHuesped.Nombre,
                NumDocumento = detalleHuesped.NumDocumento,
                Activo = detalleHuesped.Activo
            };
        }
    }
}