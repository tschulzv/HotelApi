using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;
using System.Runtime.ConstrainedExecution;

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
        public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetSolicitud()
        {
            var sol = await _context.Solicitud.Where(s => s.Activo).ToListAsync();
            var solDto = sol.Select(s => ToDTO(s));
            return Ok(solDto);
        }

        // GET: api/Solicitudes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudDTO>> GetSolicitud(int id)
        {
            var solicitud = await _context.Solicitud.Where(s => s.Activo && s.Id == id).FirstOrDefaultAsync();

            if (solicitud == null)
            {
                return NotFound();
            }

            return ToDTO(solicitud);
        }

        // PUT: api/Solicitudes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, SolicitudDTO solDto)
        {
            if (id != solDto.Id)
            {
                return BadRequest();
            }

            var sol = await _context.Solicitud.FindAsync(id);

            if (sol == null)
            {
                return NotFound();
            }

            sol.ReservaId = solDto.Id;
            sol.CancelacionId = solDto.CancelacionId;
            sol.ConsultaId = solDto.ConsultaId;
            sol.EsLeida = solDto.EsLeida;
            sol.Actualizacion = DateTime.Now;

            _context.Entry(sol).State = EntityState.Modified;

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
        public async Task<ActionResult<SolicitudDTO>> PostSolicitud(SolicitudDTO solDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var solicitud = new Solicitud
            {
                ReservaId = solDTO.ReservaId,
                CancelacionId = solDTO.CancelacionId,
                ConsultaId = solDTO.ConsultaId,
                EsLeida = solDTO.EsLeida,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.Solicitud.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitud.Id }, solDTO);
        }

        // DELETE: api/Solicitudes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var sol = await _context.Solicitud.FindAsync(id);
            if (sol == null)
            {
                return NotFound();
            }

            sol.Activo = false;
            sol.Actualizacion = DateTime.Now;
            _context.Entry(sol).State = EntityState.Modified;

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

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.Id == id);
        }

        private static SolicitudDTO ToDTO(Solicitud sol)
        {
            return new SolicitudDTO
            {
                Id = sol.Id,
                ReservaId = sol.ReservaId,
                CancelacionId = sol.CancelacionId,
                ConsultaId = sol.ConsultaId,
                EsLeida = sol.EsLeida,
                Creacion = sol.Creacion
            };
        }
    }
}
