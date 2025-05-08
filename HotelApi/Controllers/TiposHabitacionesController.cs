using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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
        var tipos = await _context.TipoHabitacion
            .Include(t => t.Servicios)
            .Where(t => t.Activo)
            .ToListAsync();

        return Ok(tipos.Select(ToDTO));
    }

    // GET: api/TiposHabitaciones/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TipoHabitacionDTO>> GetTipoHabitacion(int id)
    {
        var tipo = await _context.TipoHabitacion
            .Include(t => t.Servicios)
            .FirstOrDefaultAsync(t => t.Id == id && t.Activo);

        if (tipo == null)
            return NotFound();

        return ToDTO(tipo);
    }

    // PUT: api/TiposHabitaciones/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTipoHabitacion(int id, TipoHabitacionDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var tipo = await _context.TipoHabitacion
            .Include(t => t.Servicios)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tipo == null)
            return NotFound();

        tipo.Nombre = dto.Nombre;
        tipo.Descripcion = dto.Descripcion;
        tipo.PrecioBase = dto.PrecioBase;
        tipo.CantidadDisponible = dto.CantidadDisponible;
        tipo.MaximaOcupacion = dto.MaximaOcupacion;
        tipo.Tamanho = dto.Tamanho;
        tipo.Actualizacion = DateTime.Now;

        if (dto.Servicios != null)
        {
            var servicioIds = dto.Servicios.Select(s => s.Id).ToList();
            var nuevosServicios = await _context.Servicio
                .Where(s => servicioIds.Contains(s.Id))
                .ToListAsync();

            tipo.Servicios.Clear();
            tipo.Servicios.AddRange(nuevosServicios);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/TiposHabitaciones
    [HttpPost]
    public async Task<ActionResult<TipoHabitacionDTO>> PostTipoHabitacion(TipoHabitacionDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var servicioIds = dto.Servicios.Select(s => s.Id).ToList();
        var servicios = await _context.Servicio
            .Where(s => servicioIds.Contains(s.Id))
            .ToListAsync();


        var tipo = new TipoHabitacion
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            PrecioBase = dto.PrecioBase,
            CantidadDisponible = dto.CantidadDisponible,
            MaximaOcupacion = dto.MaximaOcupacion,
            Tamanho = dto.Tamanho,
            Servicios = servicios,
            Activo = true,
            Creacion = DateTime.Now
        };

        _context.TipoHabitacion.Add(tipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTipoHabitacion), new { id = tipo.Id }, ToDTO(tipo));
    }

    // DELETE: api/TiposHabitaciones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTipoHabitacion(int id)
    {
        var tipo = await _context.TipoHabitacion.FindAsync(id);
        if (tipo == null)
            return NotFound();

        tipo.Activo = false;
        _context.Entry(tipo).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool TipoHabitacionExists(int id) =>
        _context.TipoHabitacion.Any(e => e.Id == id);

    private static TipoHabitacionDTO ToDTO(TipoHabitacion tipo) => new TipoHabitacionDTO
    {
        Id = tipo.Id,
        Nombre = tipo.Nombre,
        Descripcion = tipo.Descripcion,
        PrecioBase = tipo.PrecioBase,
        CantidadDisponible = tipo.CantidadDisponible,
        MaximaOcupacion = tipo.MaximaOcupacion,
        Tamanho = tipo.Tamanho,
        Servicios = tipo.Servicios?.Select(s => new ServicioDTO
        {
            Id = s.Id,
            Nombre = s.Nombre,
            IconName = s.IconName
        }).ToList() ?? new List<ServicioDTO>()
    };
}
