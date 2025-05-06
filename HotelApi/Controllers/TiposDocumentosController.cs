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
using HotelApi.DTOs;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TiposDocumentosController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public TiposDocumentosController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/TiposDocumentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoDocumentoDTO>>> GetTipoDocumento()
        {
            var tipos = await _context.TipoDocumento.Where(t => t.Activo).ToListAsync();
            var dtos = tipos.Select(t => ToDTO(t));
            return Ok(dtos);
        }

        // GET: api/TiposDocumentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoDocumentoDTO>> GetTipoDocumento(int id)
        {
            var tipoDocumento = await _context.TipoDocumento.Where(t => t.Activo && t.Id == id).FirstOrDefaultAsync();

            if (tipoDocumento == null)
            {
                return NotFound();
            }

            return ToDTO(tipoDocumento);
        }

        // PUT: api/TiposDocumentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoDocumento(int id, TipoDocumentoDTO tipoDto)
        {
            if (id != tipoDto.Id)
            {
                return BadRequest();
            }

            var tipo = await _context.TipoDocumento.FindAsync(id);

            if (tipo == null)
            {
                return NotFound();
            }

            tipo.Nombre = tipoDto.Nombre;
            tipo.Actualizacion = DateTime.Now;

            _context.Entry(tipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDocumentoExists(id))
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

        // POST: api/TiposDocumentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoDocumentoDTO>> PostTipoDocumento(TipoDocumentoDTO tipoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var tipo = new TipoDocumento
            {
                Nombre = tipoDto.Nombre,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.TipoDocumento.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoDocumento", new { id = tipo.Id }, tipoDto);
        }

        // DELETE: api/TiposDocumentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoDocumento(int id)
        {
            var tipoDocumento = await _context.TipoDocumento.FindAsync(id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }

            tipoDocumento.Activo = false;
            tipoDocumento.Actualizacion = DateTime.Now;
            _context.Entry(tipoDocumento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoDocumentoExists(id))
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

        private bool TipoDocumentoExists(int id)
        {
            return _context.TipoDocumento.Any(e => e.Id == id);
        }

        private static TipoDocumentoDTO ToDTO(TipoDocumento tipo)
        {
            return new TipoDocumentoDTO
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre
            };
        }
    }
}
