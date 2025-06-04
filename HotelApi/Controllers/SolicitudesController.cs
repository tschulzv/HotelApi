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
using Microsoft.AspNetCore.Authorization;
using HotelApi.DTOs;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public SolicitudesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudDTO>>> GetSolicitud()
        {
            var sol = await _context.Solicitud.Where(s => s.Activo)
                .Include(s => s.Reserva)
                    .ThenInclude(r => r.Cliente)
                .Include(s => s.Reserva)
                    .ThenInclude(r => r.Detalles)
                        .ThenInclude(d => d.TipoHabitacion)
                .Include(s => s.Cancelacion)
                .Include(s => s.Consulta)
                .ToListAsync();
            var solDto = sol.Select(s => ToDTO(s));
            return Ok(solDto);
        }

        // GET: api/Solicitudes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudDTO>> GetSolicitud(int id)
        {
            var solicitud = await _context.Solicitud
                .Where(s => s.Activo && s.Id == id)
                .Include(s => s.Reserva)
                    .ThenInclude(r => r.Cliente)
                .Include(s => s.Reserva)
                    .ThenInclude(r => r.Detalles) 
                        .ThenInclude(d => d.TipoHabitacion)
                .Include(s => s.Cancelacion)
                .Include(s => s.Consulta)
                .FirstOrDefaultAsync();

            if (solicitud == null)
            {
                return NotFound();
            }

            return ToDTO(solicitud);
        }

        // GET: api/Solicitudes/unread 
        // devuelve true o false segun existan o no notificaciones sin leer
        [HttpGet("unread")]
        public async Task<ActionResult<object>> GetUnreadSolicitudes()
        {
            // Contamos solicitudes activas que aun no estan leidas
            var unreadCount = await _context.Solicitud
                .Where(s => s.Activo && !s.EsLeida) 
                .CountAsync();

            return Ok(new { unreadCount });
        }

        // PUT: api/Solicitudes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, SolicitudDTO solDto)
        {
            if (id != solDto.Id)
            {
                return BadRequest();
            }

            var sol = await _context.Solicitud.FindAsync(id);

            if (sol == null)
            {
                return NotFound();
            }

            sol.ReservaId = solDto.Id;
            sol.CancelacionId = solDto.CancelacionId;
            sol.ConsultaId = solDto.ConsultaId;
            sol.EsLeida = solDto.EsLeida;
            sol.Tipo = solDto.Tipo;
            sol.Actualizacion = DateTime.Now;

            _context.Entry(sol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
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

        [HttpPut("{id}/read")]
        public async Task<IActionResult> PutLeidaSolicitud(int id)
        {

            var sol = await _context.Solicitud.FindAsync(id);

            if (sol == null)
            {
                return NotFound();
            }

            sol.EsLeida = true;
            sol.Actualizacion = DateTime.Now;

            _context.Entry(sol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
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


        // POST: api/Solicitudes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolicitudDTO>> PostSolicitud(SolicitudDTO solDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }

            var solicitud = new Solicitud
            {
                ReservaId = solDTO.ReservaId,
                CancelacionId = solDTO.CancelacionId,
                ConsultaId = solDTO.ConsultaId,
                EsLeida = solDTO.EsLeida,
                Tipo = solDTO.Tipo,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true,
                Motivo = solDTO.Motivo
            };
            _context.Solicitud.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitud.Id }, solDTO);
        }

        // DELETE: api/Solicitudes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var sol = await _context.Solicitud.FindAsync(id);
            if (sol == null)
            {
                return NotFound();
            }

            sol.Activo = false;
            sol.Actualizacion = DateTime.Now;
            _context.Entry(sol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
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

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.Id == id);
        }

        private static SolicitudDTO ToDTO(Solicitud sol)
        {
            return new SolicitudDTO
            {
                Id = sol.Id,
                ReservaId = sol.ReservaId,
                Reserva = sol.ReservaId != null ? ReservasController.ToDTO(sol.Reserva) : null,
                CancelacionId = sol.CancelacionId,
                Cancelacion = sol.CancelacionId != null ? CancelacionsController.ToDTO(sol.Cancelacion) : null,
                ConsultaId = sol.ConsultaId,
                Consulta = sol.ConsultaId != null ? ConsultasController.ToDTO(sol.Consulta) : null,
                EsLeida = sol.EsLeida,
                Tipo = sol.Tipo,
                Creacion = sol.Creacion
            };
        }


    }
}
