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
    public class ClientesController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public ClientesController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetCliente()
        {
            var clientes = await _context.Cliente.ToListAsync();
            var clientesDTOs = clientes.Select(c => ToDTO(c)).ToList();
            return Ok(clientesDTOs);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return ToDTO(cliente);
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteDTO clienteDTO)
        {
            if (id != clienteDTO.Id)
            {
                return BadRequest();
            }

            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            cliente.Nombre = cliente.Nombre;
            cliente.Apellido = cliente.Apellido;
            cliente.Email = cliente.Email;
            cliente.Telefono = cliente.Telefono;
            cliente.NumDocumento = cliente.NumDocumento;
            cliente.TipoDocumentoId = cliente.TipoDocumentoId;
            cliente.Nacionalidad = cliente.Nacionalidad;
            cliente.Comentarios = cliente.Comentarios;
            cliente.Activo = cliente.Activo;
            cliente.Actualizacion = DateTime.Now;
            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        // POST: api/Clientes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                Id = clienteDTO.Id, // Considera si el ID debe ser generado por la base de datos
                Nombre = clienteDTO.Nombre,
                Apellido = clienteDTO.Apellido,
                Email = clienteDTO.Email,
                Telefono = clienteDTO.Telefono,
                NumDocumento = clienteDTO.NumDocumento,
                TipoDocumentoId = clienteDTO.TipoDocumentoId,
                Nacionalidad = clienteDTO.Nacionalidad,
                Comentarios = clienteDTO.Comentarios,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = clienteDTO.Activo
            };
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }

        private static ClienteDTO ToDTO(Cliente cliente)
        {
            return new ClienteDTO
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Email = cliente.Email,
                Telefono = cliente.Telefono,
                NumDocumento = cliente.NumDocumento,
                TipoDocumentoId = cliente.TipoDocumentoId,
                Nacionalidad = cliente.Nacionalidad,
                Comentarios = cliente.Comentarios,
                Activo = cliente.Activo
            };
        }
    }
}
