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
    public class PensionesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public PensionesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Pensiones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pension>>> GetPension()
        {
            return await _context.Pension.ToListAsync();
        }

        // GET: api/Pensiones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pension>> GetPension(int id)
        {
            var pension = await _context.Pension.FindAsync(id);

            if (pension == null)
            {
                return NotFound();
            }

            return pension;
        }

        // PUT: api/Pensiones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPension(int id, Pension pension)
        {
            if (id != pension.Id)
            {
                return BadRequest();
            }

            _context.Entry(pension).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PensionExists(id))
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

        // POST: api/Pensiones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pension>> PostPension(Pension pension)
        {
            _context.Pension.Add(pension);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPension", new { id = pension.Id }, pension);
        }

        // DELETE: api/Pensiones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePension(int id)
        {
            var pension = await _context.Pension.FindAsync(id);
            if (pension == null)
            {
                return NotFound();
            }

            _context.Pension.Remove(pension);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PensionExists(int id)
        {
            return _context.Pension.Any(e => e.Id == id);
        }
    }
}
