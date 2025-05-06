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
using Microsoft.AspNetCore.Authorization;
using HotelApi.DTOs;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> PutImagenHabitacion(int id, ImagenHabitacionDTO imagenHabitacionDTO)
        {
            if (id != imagenHabitacionDTO.Id)
            {
                return BadRequest();
            }

            var imagenHabitacion = await _context.ImagenHabitacion.FindAsync(id);
            if (imagenHabitacion == null)
            {
                return NotFound();
            }

            imagenHabitacion.TipoHabitacionId = imagenHabitacionDTO.TipoHabitacionId;
            imagenHabitacion.Imagen = imagenHabitacionDTO.Imagen;
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

        // POST: api/ImagenHabitacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ImagenHabitacionDTO>> PostImagenHabitacion(ImagenHabitacionDTO imagenHabitacionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imagenHabitacion = new ImagenHabitacion
            {
                TipoHabitacionId = imagenHabitacionDTO.TipoHabitacionId,
                Imagen = imagenHabitacionDTO.Imagen,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.ImagenHabitacion.Add(imagenHabitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImagenHabitacion), new { id = imagenHabitacion.Id }, ToDTO(imagenHabitacion));
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

        private static ImagenHabitacionDTO ToDTO(ImagenHabitacion imagenHabitacion)
        {
            return new ImagenHabitacionDTO
            {
                Id = imagenHabitacion.Id,
                TipoHabitacionId = imagenHabitacion.TipoHabitacionId,
                Imagen = imagenHabitacion.Imagen
            };
        }
    }
}

