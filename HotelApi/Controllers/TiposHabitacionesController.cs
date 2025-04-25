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
    public class TiposHabitacionesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public TiposHabitacionesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/TiposHabitaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoHabitacion>>> GetTipoHabitacion()
        {
            return await _context.TipoHabitacion.ToListAsync();
        }

        // GET: api/TiposHabitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoHabitacion>> GetTipoHabitacion(int id)
        {
            var tipoHabitacion = await _context.TipoHabitacion.FindAsync(id);

            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            return tipoHabitacion;
        }

        // PUT: api/TiposHabitaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoHabitacion(int id, TipoHabitacion tipoHabitacion)
        {
            if (id != tipoHabitacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(tipoHabitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoHabitacionExists(id))
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

        // POST: api/TiposHabitaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoHabitacion>> PostTipoHabitacion(TipoHabitacion tipoHabitacion)
        {
            _context.TipoHabitacion.Add(tipoHabitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoHabitacion", new { id = tipoHabitacion.Id }, tipoHabitacion);
        }

        // DELETE: api/TiposHabitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoHabitacion(int id)
        {
            var tipoHabitacion = await _context.TipoHabitacion.FindAsync(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            _context.TipoHabitacion.Remove(tipoHabitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoHabitacionExists(int id)
        {
            return _context.TipoHabitacion.Any(e => e.Id == id);
        }
    }
}
