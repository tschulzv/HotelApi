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
    public class PensionesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public PensionesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Pensiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PensionDTO>>> GetPension()
        {
            var pensiones = await _context.Pension.ToListAsync();
            var pensionesDtos = pensiones.Select(p => ToDTO(p));
            return Ok(pensionesDtos);
        }

        // GET: api/Pensiones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PensionDTO>> GetPension(int id)
        {
            var pension = await _context.Pension.FindAsync(id);

            if (pension == null)
            {
                return NotFound();
            }

            return ToDTO(pension);
        }

        // PUT: api/Pensiones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPension(int id, PensionDTO pensionDto)
        {
            if (id != pensionDto.Id)
            {
                return BadRequest();
            }

            var pe = await _context.Pension.FindAsync(id);

            if (pe == null)
            {
                return NotFound();
            }

            pe.Nombre = pensionDto.Nombre;
            pe.Descripcion = pensionDto.Descripcion;
            pe.PrecioAdicional = pensionDto.PrecioAdicional;
            pe.Actualizacion = DateTime.Now;

            _context.Entry(pe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionExists(id))
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

        // POST: api/Pensiones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pension>> PostPension(PensionDTO pensionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }

            var pe = new Pension
            {
                Nombre = pensionDto.Nombre,
                Descripcion = pensionDto.Descripcion,
                PrecioAdicional = pensionDto.PrecioAdicional,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.Pension.Add(pe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPension", new { id = pe.Id }, pensionDto);
        }

        // DELETE: api/Pensiones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePension(int id)
        {
            var pension = await _context.Pension.FindAsync(id);
            if (pension == null)
            {
                return NotFound();
            }

            pension.Activo = false;
            pension.Actualizacion = DateTime.Now;

            _context.Entry(pension).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionExists(id))
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

        private bool PensionExists(int id)
        {
            return _context.Pension.Any(e => e.Id == id);
        }

        private static PensionDTO ToDTO(Pension p)
        {
            return new PensionDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioAdicional = p.PrecioAdicional
            };
        }
    }
}
