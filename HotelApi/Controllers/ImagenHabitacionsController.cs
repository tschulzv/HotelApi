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
    public class ImagenHabitacionsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public ImagenHabitacionsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/ImagenHabitacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImagenHabitacion>>> GetImagenHabitacion()
        {
            return await _context.ImagenHabitacion.ToListAsync();
        }

        // GET: api/ImagenHabitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImagenHabitacion>> GetImagenHabitacion(int id)
        {
            var imagenHabitacion = await _context.ImagenHabitacion.FindAsync(id);

            if (imagenHabitacion == null)
            {
                return NotFound();
            }

            return imagenHabitacion;
        }

        // PUT: api/ImagenHabitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagenHabitacion(int id, ImagenHabitacion imagenHabitacion)
        {
            if (id != imagenHabitacion.Id)
            {
                return BadRequest();
            }

            _context.Entry(imagenHabitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenHabitacionExists(id))
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

        // POST: api/ImagenHabitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImagenHabitacion>> PostImagenHabitacion(ImagenHabitacion imagenHabitacion)
        {
            _context.ImagenHabitacion.Add(imagenHabitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagenHabitacion", new { id = imagenHabitacion.Id }, imagenHabitacion);
        }

        // DELETE: api/ImagenHabitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagenHabitacion(int id)
        {
            var imagenHabitacion = await _context.ImagenHabitacion.FindAsync(id);
            if (imagenHabitacion == null)
            {
                return NotFound();
            }

            _context.ImagenHabitacion.Remove(imagenHabitacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagenHabitacionExists(int id)
        {
            return _context.ImagenHabitacion.Any(e => e.Id == id);
        }
    }
}
