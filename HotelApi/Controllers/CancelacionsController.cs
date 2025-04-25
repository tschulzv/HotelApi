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
            var cancelaciones = await _context.Cancelacion.ToListAsync();
            var cancelacionDTOs = cancelaciones.Select(ca => ToDTO(ca));
            return Ok(cancelacionDTOs);
        }

        // GET: api/Cancelacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CancelacionDTO>> GetCancelacion(int id)
        {
            var cancelacion = await _context.Cancelacion.FindAsync(id);

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
            {
                return BadRequest(ModelState);
            }
            var cancelacion = new Cancelacion {
                DetalleReservaId = caDto.DetalleReservaId,
                Motivo = caDto.Motivo,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.Cancelacion.Add(cancelacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCancelacion", new { id = cancelacion.Id }, caDto);
        }

        // DELETE: api/Cancelacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCancelacion(int id)
        {
            var cancelacion = await _context.Cancelacion.FindAsync(id);
            if (cancelacion == null)
            {
                return NotFound();
            }

            _context.Cancelacion.Remove(cancelacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CancelacionExists(int id)
        {
            return _context.Cancelacion.Any(e => e.Id == id);
        }

        private static CancelacionDTO ToDTO(Cancelacion ca)
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
