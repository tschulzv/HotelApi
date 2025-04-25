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
    public class ReservasController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public ReservasController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaDTO>>> GetReservas()
        {
            var res = await _context.Reserva.ToListAsync();
            var resDtos = res.Select(r => ToDTO(r));
            return Ok(resDtos);
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDTO>> GetReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            return ToDTO(reserva);
        }

        // PUT: api/Reservas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, ReservaDTO resDTO)
        {
            if (id != resDTO.Id)
            {
                return BadRequest();
            }

            var res = await _context.Reserva.FindAsync(id);

            if (res == null)
            {
                return NotFound();
            }

            res.ClienteId = resDTO.ClienteId;
            res.Codigo = resDTO.Codigo;
            res.FechaIngreso = resDTO.FechaIngreso;
            res.FechaSalida = resDTO.FechaSalida;
            res.LlegadaEstimada = resDTO.LlegadaEstimada;
            res.Comentarios = resDTO.Comentarios;
            res.EstadoId = resDTO.EstadoId;
            res.Actualizacion = DateTime.Now;

            _context.Entry(res).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReservaDTO>> PostReserva(ReservaDTO resDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve los errores de validación al cliente
            }
            var res = new Reserva
            {
                ClienteId = resDto.ClienteId,
                Codigo = resDto.Codigo,
                FechaIngreso = resDto.FechaIngreso,
                FechaSalida = resDto.FechaSalida,
                LlegadaEstimada = resDto.LlegadaEstimada,
                Comentarios = resDto.Comentarios,
                EstadoId = resDto.EstadoId,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true,
            };

            if (resDto.Detalles != null)
            {
                res.Detalles = resDto.Detalles.Select(d => new DetalleReserva
                {
                    ReservaId = res.Id,
                    HabitacionId = d.HabitacionId,
                    CantidadAdultos = d.CantidadAdultos,
                    CantidadNinhos = d.CantidadNinhos,
                    PensionId = d.PensionId,
                    Activo = d.Activo
                }).ToList();
            }

            _context.Reserva.Add(res);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReserva", new { id = res.Id }, resDto);
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            reserva.Activo = false;
            reserva.Actualizacion = DateTime.Now;

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        private bool ReservaExists(int id)
        {
            return _context.Reserva.Any(e => e.Id == id);
        }

        private static ReservaDTO ToDTO(Reserva re)
        {
            return new ReservaDTO
            {
                Id = re.Id,
                ClienteId = re.ClienteId,
                Codigo = re.Codigo,
                FechaIngreso = re.FechaIngreso,
                FechaSalida = re.FechaSalida,
                LlegadaEstimada = re.LlegadaEstimada,
                Comentarios = re.Comentarios,
                EstadoId = re.EstadoId,
                Detalles = re.Detalles?.Select(d => new DetalleReservaDTO
                {
                    Id = d.Id,
                    ReservaId = d.ReservaId,
                    HabitacionId = d.HabitacionId,
                    CantidadAdultos = d.CantidadAdultos,
                    CantidadNinhos = d.CantidadNinhos,
                    PensionId = d.PensionId,
                    Activo = d.Activo
                }).ToList()
            };
        }
    }
}
