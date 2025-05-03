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
    public class HabitacionsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public HabitacionsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Habitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitacionDTO>>> GetHabitacion()
        {
            var habitaciones = await _context.Habitacion.Where(h => h.Activo).ToListAsync();
            return habitaciones.Select(ToDTO).ToList();
        }

        // GET: api/Habitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HabitacionDTO>> GetHabitacion(int id)
        {
            var habitacion = await _context.Habitacion.Where(h => h.Activo && h.Id == id).FirstOrDefaultAsync();

            if (habitacion == null)
            {
                return NotFound();
            }

            return ToDTO(habitacion);
        }

        // PUT: api/Habitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHabitacion(int id, HabitacionDTO habitacionDTO)
        {
            if (id != habitacionDTO.Id)
            {
                return BadRequest();
            }

            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            habitacion.TipoHabitacionId = habitacionDTO.TipoHabitacionId;
            habitacion.NumeroHabitacion = habitacionDTO.NumeroHabitacion;
            habitacion.EstadoHabitacionId= habitacionDTO.EstadoHabitacionId;
            habitacion.Activo = habitacionDTO.Activo;
            habitacion.Actualizacion = DateTime.Now; // Actualizar la fecha de actualización

            _context.Entry(habitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
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

        // POST: api/Habitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HabitacionDTO>> PostHabitacion(HabitacionDTO habitacionDTO)
        {
            var habitacion = new Habitacion
            {
                TipoHabitacionId = habitacionDTO.TipoHabitacionId,
                NumeroHabitacion = habitacionDTO.NumeroHabitacion,
                EstadoHabitacionId = habitacionDTO.EstadoHabitacionId,
                Activo = habitacionDTO.Activo,
                Creacion = DateTime.Now,       // Establecer la fecha de creación
                Actualizacion = DateTime.Now    // Establecer la fecha de actualización
            };

            _context.Habitacion.Add(habitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHabitacion), new { id = habitacion.Id }, ToDTO(habitacion));
        }

        // DELETE: api/Habitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }

            _context.Habitacion.Remove(habitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitacion.Any(e => e.Id == id);
        }

        private static HabitacionDTO ToDTO(Habitacion habitacion)
        {
            return new HabitacionDTO
            {
                Id = habitacion.Id,
                TipoHabitacionId = habitacion.TipoHabitacionId,
                NumeroHabitacion = habitacion.NumeroHabitacion,
                EstadoHabitacionId = habitacion.EstadoHabitacionId,
                Activo = habitacion.Activo
            };
        }
    }
}
