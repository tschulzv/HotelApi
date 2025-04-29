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
    public class TiposHabitacionesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public TiposHabitacionesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/TiposHabitaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoHabitacionDTO>>> GetTipoHabitacion()
        {
            var tipos = await _context.TipoHabitacion.ToListAsync();
            var dtos = tipos.Select(t => ToDTO(t));
            return Ok(dtos);
        }

        // GET: api/TiposHabitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoHabitacionDTO>> GetTipoHabitacion(int id)
        {
            var tipoHabitacion = await _context.TipoHabitacion.FindAsync(id);

            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            return ToDTO(tipoHabitacion);
        }

        // PUT: api/TiposHabitaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoHabitacion(int id, TipoHabitacionDTO tipoDto)
        {
            if (id != tipoDto.Id)
            {
                return BadRequest();
            }

            var tipo = await _context.TipoHabitacion.FindAsync(id);

            if (tipo == null)
            {
                return NotFound();
            }

            tipo.Nombre = tipoDto.Nombre;
            tipo.Descripcion = tipoDto.Descripcion;
            tipo.PrecioBase = tipoDto.PrecioBase;
            tipo.CantidadDisponible = tipoDto.CantidadDisponible;
            // !!! falta actualizar la lista de servicios

            _context.Entry(tipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoHabitacionExists(id))
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

        // POST: api/TiposHabitaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoHabitacionDTO>> PostTipoHabitacion(TipoHabitacionDTO tipoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var tipo = new TipoHabitacion
            {
                Nombre = tipoDto.Nombre,
                Descripcion = tipoDto.Descripcion,
                PrecioBase = tipoDto.PrecioBase,
                CantidadDisponible = tipoDto.CantidadDisponible,
                Servicios = new System.Collections.ObjectModel.Collection<Servicio>(
                    tipoDto.Servicios?.Select(dto => new Servicio
                    {
                        Nombre = dto.Nombre,
                        IconName = dto.IconName,
                        Creacion = DateTime.Now,
                        Actualizacion = DateTime.Now,
                        Activo = true,
                    }).ToList() ?? new List<Servicio>()
                )
            };
            _context.TipoHabitacion.Add(tipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoHabitacion", new { id = tipo.Id }, tipoDto);
        }

        // DELETE: api/TiposHabitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoHabitacion(int id)
        {
            var tipoHabitacion = await _context.TipoHabitacion.FindAsync(id);
            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            tipoHabitacion.Activo = false;
            _context.Entry(tipoHabitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoHabitacionExists(id))
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

        private bool TipoHabitacionExists(int id)
        {
            return _context.TipoHabitacion.Any(e => e.Id == id);
        }

        private static TipoHabitacionDTO ToDTO(TipoHabitacion tipo)
        {
            return new TipoHabitacionDTO
            {
                Id = tipo.Id,
                Nombre = tipo.Nombre,
                Descripcion = tipo.Descripcion,
                PrecioBase = tipo.PrecioBase,
                CantidadDisponible = tipo.CantidadDisponible,
                Servicios = tipo.Servicios?.Select(s => new ServicioDTO
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    IconName = s.IconName
                }).ToList()
            };
        }
    }
}
