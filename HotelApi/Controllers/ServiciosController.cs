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
    public class ServiciosController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public ServiciosController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Servicios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDTO>>> GetServicios()
        {
            var se = await _context.Servicio.Where(s => s.Activo).ToListAsync();
            var seDtos = se.Select(s => ToDTO(s));
            return Ok(seDtos);
        }


        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDTO>> GetServicio(int id)
        {
            var servicio = await _context.Servicio.Where(s => s.Activo && s.Id == id).FirstOrDefaultAsync();

            if (servicio == null)
            {
                return NotFound();
            }

            return ToDTO(servicio);
        }

        // PUT: api/Servicios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicio(int id, ServicioDTO serDto)
        {
            if (id != serDto.Id)
            {
                return BadRequest();
            }

            var ser = await _context.Servicio.FindAsync(id);

            if (ser == null)
            {
                return NotFound();
            }

            ser.Nombre = serDto.Nombre;
            ser.IconName = serDto.IconName;
            ser.Actualizacion = DateTime.Now;

            _context.Entry(ser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        // POST: api/Servicios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServicioDTO>> PostServicio(ServicioDTO serDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var servicio = new Servicio
            {
                Nombre = serDto.Nombre,
                IconName = serDto.IconName,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };

            _context.Servicio.Add(servicio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicio", new { id = servicio.Id }, serDto);
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            var ser = await _context.Servicio.FindAsync(id);
            if (ser == null)
            {
                return NotFound();
            }

            ser.Activo = false;
            ser.Actualizacion = DateTime.Now;

            _context.Entry(ser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicioExists(id))
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

        private bool ServicioExists(int id)
        {
            return _context.Servicio.Any(e => e.Id == id);
        }

        private static ServicioDTO ToDTO(Servicio s)
        {
            return new ServicioDTO
            {
                Id = s.Id,
                Nombre = s.Nombre,
                IconName = s.IconName
            };
        }
    }
}
