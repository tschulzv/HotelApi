using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoReservasController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public EstadoReservasController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/EstadoReservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoReservaDTO>>> GetEstadoReserva()
        {
            var estadoReservas = await _context.EstadoReserva.Where(e => e.Activo).ToListAsync();
            var estadoReservasDTOs = estadoReservas.Select(e => ToDTO(e)).ToList();
            return Ok(estadoReservasDTOs);
        }

        // GET: api/EstadoReservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoReservaDTO>> GetEstadoReserva(int id)
        {
            var estadoReserva = await _context.EstadoReserva.Where(e => e.Activo && e.Id == id).FirstOrDefaultAsync();

            if (estadoReserva == null)
            {
                return NotFound();
            }

            return ToDTO(estadoReserva);
        }

        // PUT: api/EstadoReservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoReserva(int id, EstadoReservaDTO estadoReservaDTO)
        {
            if (id != estadoReservaDTO.Id)
            {
                return BadRequest();
            }

            var estadoReserva = await _context.EstadoReserva.FindAsync(id);
            if (estadoReserva == null)
            {
                return NotFound();
            }

            estadoReserva.Nombre = estadoReservaDTO.Nombre;

            _context.Entry(estadoReserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoReservaExists(id))
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

        // POST: api/EstadoReservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoReservaDTO>> PostEstadoReserva(EstadoReservaDTO estadoReservaDTO)
        {
            var estadoReserva = new EstadoReserva
            {
                Nombre = estadoReservaDTO.Nombre
            };
            _context.EstadoReserva.Add(estadoReserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoReserva), new { id = estadoReserva.Id }, ToDTO(estadoReserva));
        }

        // DELETE: api/EstadoReservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoReserva(int id)
        {
            var estadoReserva = await _context.EstadoReserva.FindAsync(id);
            if (estadoReserva == null)
            {
                return NotFound();
            }

            _context.EstadoReserva.Remove(estadoReserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoReservaExists(int id)
        {
            return _context.EstadoReserva.Any(e => e.Id == id);
        }

        private static EstadoReservaDTO ToDTO(EstadoReserva estadoReserva)
        {
            return new EstadoReservaDTO
            {
                Id = estadoReserva.Id,
                Nombre = estadoReserva.Nombre
            };
        }
    }
}
