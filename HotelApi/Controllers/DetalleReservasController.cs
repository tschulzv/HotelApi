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
    public class DetalleReservasController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public DetalleReservasController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/DetalleReservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleReservaDTO>>> GetDetalleReserva()
        {
            var detalleReservas = await _context.DetalleReserva.Where(d => d.Activo).ToListAsync();
            return detalleReservas.Select(ToDTO).ToList();
        }

        // GET: api/DetalleReservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleReservaDTO>> GetDetalleReserva(int id)
        {
            var detalleReserva = await _context.DetalleReserva.Where(d => d.Activo && d.Id == id).FirstOrDefaultAsync();

            if (detalleReserva == null)
            {
                return NotFound();
            }

            return ToDTO(detalleReserva);
        }

        // PUT: api/DetalleReservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleReserva(int id, DetalleReservaDTO detalleReservaDTO)
        {
            if (id != detalleReservaDTO.Id)
            {
                return BadRequest();
            }

            var detalleReserva = await _context.DetalleReserva.FindAsync(id);
            if (detalleReserva == null)
            {
                return NotFound();
            }

            detalleReserva.ReservaId = detalleReservaDTO.ReservaId;
            detalleReserva.HabitacionId = detalleReservaDTO.HabitacionId;
            detalleReserva.CantidadAdultos = detalleReservaDTO.CantidadAdultos;
            detalleReserva.CantidadNinhos = detalleReservaDTO.CantidadNinhos;
            detalleReserva.PensionId = detalleReservaDTO.PensionId;
            detalleReserva.Activo = detalleReservaDTO.Activo;
            detalleReserva.Actualizacion = DateTime.Now;

            _context.Entry(detalleReserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleReservaExists(id))
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

        // POST: api/DetalleReservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetalleReservaDTO>> PostDetalleReserva(DetalleReservaDTO detalleReservaDTO)
        {
            var detalleReserva = new DetalleReserva
            {
                ReservaId = detalleReservaDTO.ReservaId,
                HabitacionId = detalleReservaDTO.HabitacionId,
                CantidadAdultos = detalleReservaDTO.CantidadAdultos,
                CantidadNinhos = detalleReservaDTO.CantidadNinhos,
                PensionId = detalleReservaDTO.PensionId,
                Activo = detalleReservaDTO.Activo,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now
            };
            _context.DetalleReserva.Add(detalleReserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDetalleReserva), new { id = detalleReserva.Id }, ToDTO(detalleReserva));
        }

        // DELETE: api/DetalleReservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleReserva(int id)
        {
            var detalleReserva = await _context.DetalleReserva.FindAsync(id);
            if (detalleReserva == null)
            {
                return NotFound();
            }

            _context.DetalleReserva.Remove(detalleReserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleReservaExists(int id)
        {
            return _context.DetalleReserva.Any(e => e.Id == id);
        }

        private static DetalleReservaDTO ToDTO(DetalleReserva detalleReserva)
        {
            return new DetalleReservaDTO
            {
                Id = detalleReserva.Id,
                ReservaId = detalleReserva.ReservaId,
                HabitacionId = detalleReserva.HabitacionId,
                CantidadAdultos = detalleReserva.CantidadAdultos,
                CantidadNinhos = detalleReserva.CantidadNinhos,
                PensionId = detalleReserva.PensionId,
                Activo = detalleReserva.Activo
            };
        }
    }
}