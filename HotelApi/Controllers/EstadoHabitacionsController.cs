using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;
using HotelApi.DTOs;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoHabitacionsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public EstadoHabitacionsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/EstadoHabitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoHabitacion>>> GetEstadoHabitacion()
        {
            var estados = await _context.EstadoHabitacion
            .Where(e => e.Activo)
            .ToListAsync();

            var dtoList = estados.Select(ToDTO).ToList();

            return Ok(dtoList);
        }

        // GET: api/EstadoHabitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoHabitacionDTO>> GetEstadoHabitacion(int id)
        {
            var estado = await _context.EstadoHabitacion
            .Where(e => e.Id == id && e.Activo)
            .FirstOrDefaultAsync();

            if (estado == null)
                return NotFound();

            return ToDTO(estado);
        }

        // PUT: api/EstadoHabitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoHabitacion(int id, EstadoHabitacionDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var estado = await _context.EstadoHabitacion.FindAsync(id);
            if (estado == null || !estado.Activo)
                return NotFound();

            estado.Nombre = dto.Nombre;
            estado.Actualizacion = DateTime.Now;

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoHabitacionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/EstadoHabitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoHabitacion>> PostEstadoHabitacion(EstadoHabitacionDTO dto)
        {
            var estado = new EstadoHabitacion
            {
                Nombre = dto.Nombre,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };

            _context.EstadoHabitacion.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstadoHabitacion), new { id = estado.Id }, ToDTO(estado));
        }

        // DELETE: api/EstadoHabitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoHabitacion(int id)
        {
            var estadoHabitacion = await _context.EstadoHabitacion.FindAsync(id);
            if (estadoHabitacion == null || !estadoHabitacion.Activo)
                return NotFound();

            estadoHabitacion.Activo = false;
            estadoHabitacion.Actualizacion = DateTime.Now;
            _context.Entry(estadoHabitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoHabitacionExists(id))
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

        private bool EstadoHabitacionExists(int id)
        {
            return _context.EstadoHabitacion.Any(e => e.Id == id);
        }

        private static EstadoHabitacionDTO ToDTO(EstadoHabitacion estado)
        {
            return new EstadoHabitacionDTO
            {
                Id = estado.Id,
                Nombre = estado.Nombre
            };
        }
    }
}
