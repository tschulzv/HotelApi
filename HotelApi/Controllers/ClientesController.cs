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
            var clientes = await _context.Cliente.Where(c => c.Activo).Include(c => c.TipoDocumento).ToListAsync();
            var clientesDTOs = clientes.Select(c => ToDTO(c)).ToList();
            return Ok(clientesDTOs);
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.Where(c => c.Activo && c.Id == id).Include(c => c.TipoDocumento).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }

            return ToDTO(cliente);
        }

        [HttpGet("document/{tipoDocumentoId}/{numDocumento}")]
        public async Task<ActionResult<ClienteDTO>> GetClienteDocumento(int tipoDocumentoId, string numDocumento)
        {
            var cliente = await _context.Cliente.Where(c => c.Activo && c.TipoDocumentoId == tipoDocumentoId && c.NumDocumento == numDocumento).Include(c => c.TipoDocumento).FirstOrDefaultAsync();

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
            cliente.Nombre = clienteDTO.Nombre;
            cliente.Apellido = clienteDTO.Apellido;
            cliente.Email = clienteDTO.Email;
            cliente.Telefono = clienteDTO.Telefono;
            cliente.NumDocumento = clienteDTO.NumDocumento;
            cliente.TipoDocumentoId = clienteDTO.TipoDocumentoId;
            cliente.Ruc = clienteDTO.Ruc;
            cliente.Nacionalidad = clienteDTO.Nacionalidad;
            cliente.Comentarios = clienteDTO.Comentarios;
            cliente.Activo = clienteDTO.Activo;
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
                Nombre = clienteDTO.Nombre,
                Apellido = clienteDTO.Apellido,
                Email = clienteDTO.Email,
                Telefono = clienteDTO.Telefono,
                NumDocumento = clienteDTO.NumDocumento,
                TipoDocumentoId = clienteDTO.TipoDocumentoId,
                Ruc = clienteDTO.Ruc,
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
            if (cliente == null || !cliente.Activo)
            {
                return NotFound();
            }

            cliente.Activo = false;
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
                TipoDocumento = cliente.TipoDocumento?.Nombre,
                Ruc = cliente.Ruc,
                Nacionalidad = cliente.Nacionalidad,
                Comentarios = cliente.Comentarios,
                Activo = cliente.Activo,
                Creacion = cliente.Creacion,
            };
        }
    }
}
