using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;
using System.Numerics;

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
        public async Task<ActionResult<IEnumerable<ImagenHabitacionDTO>>> GetImagenHabitacion()
        {
            var imagenes = await _context.ImagenHabitacion.Where(i => i.Activo).ToListAsync();
            var imgDTO = imagenes.Select(i => ToDTO(i));
            return Ok(imgDTO);
        }

        // GET: api/ImagenHabitacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImagenHabitacionDTO>> GetImagenHabitacion(int id)
        {
            var imagenHabitacion = await _context.ImagenHabitacion.Where(i => i.Activo && i.Id == id).FirstOrDefaultAsync();

            if (imagenHabitacion == null)
            {
                return NotFound();
            }

            return ToDTO(imagenHabitacion);
        }

        // PUT: api/ImagenHabitacions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagenHabitacion(int id, ImagenHabitacionDTO imgDTO)
        {
            if (id != imgDTO.Id)
            {
                return BadRequest();
            }

            var img = await _context.ImagenHabitacion.FindAsync(id);

            if (img == null)
            {
                return NotFound();
            }

            img.TipoHabitacionId = imgDTO.TipoHabitacionId;
            img.Imagen = imgDTO.Imagen;
            img.Actualizacion = DateTime.Now;

            _context.Entry(img).State = EntityState.Modified;

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
        public async Task<ActionResult<ImagenHabitacion>> PostImagenHabitacion(ImagenHabitacionDTO imgDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var img = new ImagenHabitacion
            {
                TipoHabitacionId = imgDTO.TipoHabitacionId,
                Imagen = imgDTO.Imagen,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.ImagenHabitacion.Add(img);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImagenHabitacion", new { id = img.Id }, imgDTO);
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

            imagenHabitacion.Activo = false;
            imagenHabitacion.Actualizacion = DateTime.Now;

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

        private bool ImagenHabitacionExists(int id)
        {
            return _context.ImagenHabitacion.Any(e => e.Id == id);
        }

        private static ImagenHabitacionDTO ToDTO(ImagenHabitacion img)
        {
            return new ImagenHabitacionDTO
            {
                Id = img.Id,
                TipoHabitacionId = img.TipoHabitacionId,
                Imagen = img.Imagen
            };
        }
    }
}
