using HotelApi.Data;
using HotelApi.DTOs;
using HotelApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting; // Necesario para IWebHostEnvironment
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TiposHabitacionesController : ControllerBase
{
    private readonly HotelApiContext _context;
    private readonly IWebHostEnvironment _environment; // Para acceder al sistema de archivos

    public TiposHabitacionesController(HotelApiContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
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

    // POST: api/TiposHabitaciones/ConImagenes
    [HttpPost("ConImagenes")]
    public async Task<ActionResult<TipoHabitacionDTO>> PostTipoHabitacionConImagenes([FromForm] TipoHabitacionConImagenesDTO tipoHabitacionDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var servicios = await _context.Servicio
        .Where(s => tipoHabitacionDTO.Servicios.Contains(s.Id))
        .ToListAsync();


        var tipoHabitacion = new TipoHabitacion
        {
            Nombre = tipoHabitacionDTO.Nombre,
            Descripcion = tipoHabitacionDTO.Descripcion,
            PrecioBase = tipoHabitacionDTO.PrecioBase,
            CantidadDisponible = tipoHabitacionDTO.CantidadDisponible,
            MaximaOcupacion = tipoHabitacionDTO.MaximaOcupacion,
            Tamanho = tipoHabitacionDTO.Tamanho,
            Servicios = servicios,
            Activo = true,
            Creacion = DateTime.Now
        };

        _context.TipoHabitacion.Add(tipoHabitacion);
        await _context.SaveChangesAsync(); // Guardar para obtener el Id

        if (tipoHabitacionDTO.Imagenes != null && tipoHabitacionDTO.Imagenes.Any())
        {
            foreach (var imagen in tipoHabitacionDTO.Imagenes)
            {
                if (imagen.Length > 0)
                {
                    // Guardar la imagen en el sistema de archivos (ejemplo)
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    var rutaGuardado = Path.Combine(_environment.WebRootPath, "imagenes", nombreArchivo);

                    using (var stream = new FileStream(rutaGuardado, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    // Guardar la referencia de la imagen en la base de datos
                    var imagenHabitacion = new ImagenHabitacion
                    {
                        TipoHabitacionId = tipoHabitacion.Id,
                        Imagen = await System.IO.File.ReadAllBytesAsync(rutaGuardado), // Guardar como byte array
                        Creacion = DateTime.Now,
                        Actualizacion = DateTime.Now,
                        Activo = true
                    };
                    _context.ImagenHabitacion.Add(imagenHabitacion);
                }
            }
            await _context.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(GetTipoHabitacion), new { id = tipoHabitacion.Id }, ToDTO(tipoHabitacion));
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

public class TipoHabitacionConImagenesDTO
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int CantidadDisponible { get; set; }
    public int MaximaOcupacion { get; set; }
    public int Tamanho { get; set; }
    public List<int> Servicios { get; set; }
    public List<IFormFile>? Imagenes { get; set; }
}