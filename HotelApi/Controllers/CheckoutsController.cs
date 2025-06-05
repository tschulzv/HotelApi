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
    public class CheckoutsController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public CheckoutsController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Checkouts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CheckoutDTO>>> GetCheckout()
        {
            var checkouts = await _context.Checkout.Where(c => c.Activo).ToListAsync();
            var checkoutsDTOs = checkouts.Select(c => ToDTO(c)).ToList();
            return Ok(checkoutsDTOs);
        }

        // GET: api/Checkouts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckoutDTO>> GetCheckout(int id)
        {
            var checkout = await _context.Checkout.Where(c => c.Activo && c.Id == id).FirstOrDefaultAsync();

            if (checkout == null)
            {
                return NotFound();
            }

            return ToDTO(checkout);
        }

        [HttpGet("verificarReserva/{codigo}")]
        public async Task<IActionResult> VerificarReserva(string codigo)
        {
            Console.WriteLine(codigo);
            var reserva = await _context.Reserva
                .Include(r => r.Checkin)
                .FirstOrDefaultAsync(r => r.Codigo == codigo && r.Activo);

            bool tieneCheckin = reserva != null && reserva.Checkin != null && reserva.Checkin.Activo;

            return Ok(new { success = tieneCheckin });
        }

        // PUT: api/Checkouts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCheckout(int id, CheckoutDTO checkoutDTO)
        {
            if (id != checkoutDTO.Id)
            {
                return BadRequest();
            }

            var checkout = await _context.Checkout.FindAsync(id);
            if (checkout == null)
            {
                return NotFound();
            }

            checkout.ReservaId = checkoutDTO.ReservaId;
            checkout.Activo = checkoutDTO.Activo;
            // Mapea otras propiedades actualizables
            _context.Entry(checkoutDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckoutExists(id))
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

        // POST: api/Checkouts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CheckoutDTO>> PostCheckout(CheckoutDTO checkoutDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- IDs de Estado ---
            const int ID_ESTADO_RESERVA_CHECKOUT = 5;
            const int ID_ESTADO_RESERVA_CHECKIN = 4;
            const int ID_ESTADO_HABITACION_DISPONIBLE = 1;

            var reserva = await _context.Reserva
                .Include(r => r.Detalles)
                    .ThenInclude(dr => dr.Habitacion)
                .FirstOrDefaultAsync(r => r.Codigo == checkoutDTO.Codigo && r.Activo);

            if (reserva == null)
            {
                return NotFound(new { Mensaje = $"Reserva con código {checkoutDTO.Codigo} no encontrada o no está activa." });
            }

            if (reserva.EstadoId != ID_ESTADO_RESERVA_CHECKIN)
            {
                return BadRequest(new { Mensaje = $"No se puede hacer Check-Out. La reserva (código: {reserva.Codigo}) no está en estado Check-In (actualmente está en estado ID: {reserva.EstadoId})." });
            }

            if (reserva.Detalles != null && reserva.Detalles.Any())
            {
                foreach (var detalleReserva in reserva.Detalles)
                {
                    if (detalleReserva.Habitacion != null)
                    {
                        detalleReserva.Habitacion.EstadoHabitacionId = ID_ESTADO_HABITACION_DISPONIBLE;
                        detalleReserva.Habitacion.Actualizacion = DateTime.Now;
                        _context.Entry(detalleReserva.Habitacion).State = EntityState.Modified;
                    }
                    else
                    {
                        Console.WriteLine($"Advertencia: DetalleReserva ID {detalleReserva.Id} para Reserva ID {reserva.Id} no tiene una Habitación asociada.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Advertencia: La reserva código {reserva.Codigo} (en Check-In) no tiene detalles de habitación para actualizar a disponible.");
            }
            reserva.EstadoId = ID_ESTADO_RESERVA_CHECKOUT;
            reserva.Actualizacion = DateTime.Now;
            _context.Entry(reserva).State = EntityState.Modified;


           
            var checkout = new Checkout 
            {
                ReservaId = reserva.Id,
                Activo = true,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now
            };

            _context.Checkout.Add(checkout);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { Mensaje = "Error al guardar los datos del check-out.", Detalle = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex) // Captura general
            {
                return StatusCode(500, new { Mensaje = "Ocurrió un error inesperado durante el check-out.", Detalle = ex.Message });
            }

            // Preparar el DTO de respuesta. El checkoutDTO original podría no tener el Id generado.
            var respuestaDto = new CheckoutDTO
            {
                Id = checkout.Id,
                ReservaId = checkout.ReservaId,
                Activo = checkout.Activo
            };

            return CreatedAtAction(nameof(GetCheckout), new { id = checkout.Id }, respuestaDto);
        }

        // DELETE: api/Checkouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckout(int id)
        {
            var checkout = await _context.Checkout.FindAsync(id);
            if (checkout == null || !checkout.Activo)
            {
                return NotFound();
            }

            checkout.Activo = false;
            checkout.Actualizacion = DateTime.Now;

            _context.Entry(checkout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckoutExists(id))
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

        private bool CheckoutExists(int id)
        {
            return _context.Checkout.Any(e => e.Id == id);
        }

        private static CheckoutDTO ToDTO(Checkout ci)
        {
            return new CheckoutDTO
            {
                Id = ci.Id,
                ReservaId = ci.ReservaId,
                Activo = ci.Activo
            };
        }
    }
}
