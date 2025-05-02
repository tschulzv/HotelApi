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
            var tipos = await _context.TipoHabitacion.Where(t => t.Activo).ToListAsync();
            var dtos = tipos.Select(t => ToDTO(t));
            return Ok(dtos);
        }

        // GET: api/TiposHabitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoHabitacionDTO>> GetTipoHabitacion(int id)
        {
            var tipoHabitacion = await _context.TipoHabitacion.Where(t => t.Activo && t.Id == id).FirstOrDefaultAsync();

            if (tipoHabitacion == null)
            {
                return NotFound();
            }

            return ToDTO(tipoHabitacion);
        }

        
         // asumir que no se puede agregar un servicio inexistente, es decir se deben crear los servicios con un post de servicio
        // PUT: api/TiposHabitaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoHabitacion(int id, TipoHabitacionDTO tipoDto)
        {
            if (id != tipoDto.Id)
            {
                return BadRequest();
            }

            var tipo = await _context.TipoHabitacion
                .Include(t => t.Servicios) // Trae los servicios relacionados
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tipo == null)
            {
                return NotFound();
            }

            // Actualizar propiedades simples
            tipo.Nombre = tipoDto.Nombre;
            tipo.Descripcion = tipoDto.Descripcion;
            tipo.PrecioBase = tipoDto.PrecioBase;
            tipo.CantidadDisponible = tipoDto.CantidadDisponible;
            tipo.Actualizacion = DateTime.Now;

            // Sincronizar lista de servicios
            if (tipoDto.Servicios != null)
            {
                // Obtener todos los servicios que vienen en el DTO desde la base de datos
                var nuevosServicios = await _context.Servicio
                    .Where(s => tipoDto.Servicios.Select(d => d.Id).Contains(s.Id))
                    .ToListAsync();

                // Limpiar y reemplazar
                tipo.Servicios.Clear();
                foreach (var servicio in nuevosServicios)
                {
                    tipo.Servicios.Add(servicio);
                }
            }

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
