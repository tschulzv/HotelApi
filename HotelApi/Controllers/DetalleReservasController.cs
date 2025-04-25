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
    public class DetalleReservasController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public DetalleReservasController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/DetalleReservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleReserva>>> GetDetalleReserva()
        {
            return await _context.DetalleReserva.ToListAsync();
        }

        // GET: api/DetalleReservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleReserva>> GetDetalleReserva(int id)
        {
            var detalleReserva = await _context.DetalleReserva.FindAsync(id);

            if (detalleReserva == null)
            {
                return NotFound();
            }

            return detalleReserva;
        }

        // PUT: api/DetalleReservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleReserva(int id, DetalleReserva detalleReserva)
        {
            if (id != detalleReserva.Id)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<DetalleReserva>> PostDetalleReserva(DetalleReserva detalleReserva)
        {
            _context.DetalleReserva.Add(detalleReserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleReserva", new { id = detalleReserva.Id }, detalleReserva);
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
    }
}
