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
    public class SolicitudesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public SolicitudesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solicitud>>> GetSolicitud()
        {
            return await _context.Solicitud.ToListAsync();
        }

        // GET: api/Solicitudes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Solicitud>> GetSolicitud(int id)
        {
            var solicitud = await _context.Solicitud.FindAsync(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            return solicitud;
        }

        // PUT: api/Solicitudes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, Solicitud solicitud)
        {
            if (id != solicitud.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitud).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
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

        // POST: api/Solicitudes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Solicitud>> PostSolicitud(Solicitud solicitud)
        {
            _context.Solicitud.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitud.Id }, solicitud);
        }

        // DELETE: api/Solicitudes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.Solicitud.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            _context.Solicitud.Remove(solicitud);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.Id == id);
        }
    }
}
