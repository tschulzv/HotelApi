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
            var checkouts = await _context.Checkout.ToListAsync();
            var checkoutsDTOs = checkouts.Select(c => ToDTO(c)).ToList();
            return Ok(checkoutsDTOs);
        }

        // GET: api/Checkouts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CheckoutDTO>> GetCheckout(int id)
        {
            var checkout = await _context.Checkout.FindAsync(id);

            if (checkout == null)
            {
                return NotFound();
            }

            return ToDTO(checkout);
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
            var checkout = new Checkout
            {
                Id = checkoutDTO.Id,
                ReservaId = checkoutDTO.ReservaId,
                Activo = checkoutDTO.Activo
            };
            _context.Checkout.Add(checkout);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCheckout", new { id = checkout.Id }, checkout);
        }

        // DELETE: api/Checkouts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCheckout(int id)
        {
            var checkout = await _context.Checkout.FindAsync(id);
            if (checkout == null)
            {
                return NotFound();
            }

            _context.Checkout.Remove(checkout);
            await _context.SaveChangesAsync();

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
