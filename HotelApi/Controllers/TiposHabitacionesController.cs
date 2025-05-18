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
            .Include(t => t.ImagenesHabitaciones)
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
            .Include(t => t.ImagenesHabitaciones)
            .FirstOrDefaultAsync(t => t.Id == id && t.Activo);

        if (tipo == null)
            return NotFound();

        return ToDTO(tipo);
    }

    // PUT: api/TiposHabitaciones/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTipoHabitacion(int id, [FromForm] TipoHabitacionConImagenesDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var tipo = await _context.TipoHabitacion
            .Include(t => t.Servicios)
            .Include(t => t.ImagenesHabitaciones)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tipo == null)
            return NotFound();

        // Actualizar campos
        tipo.Nombre = dto.Nombre;
        tipo.Descripcion = dto.Descripcion;
        tipo.PrecioBase = dto.PrecioBase;
        tipo.CantidadDisponible = dto.CantidadDisponible;
        tipo.MaximaOcupacion = dto.MaximaOcupacion;
        tipo.Tamanho = dto.Tamanho;
        tipo.Actualizacion = DateTime.Now;

        // Actualizar servicios
        var nuevosServicios = await _context.Servicio
            .Where(s => dto.Servicios.Contains(s.Id))
            .ToListAsync();

        tipo.Servicios.Clear();
        tipo.Servicios.AddRange(nuevosServicios);

        // Guardar nuevas imágenes
        var carpeta = Path.Combine(_environment.WebRootPath, "imagenes");
        Directory.CreateDirectory(carpeta);

        foreach (var formFile in dto.Imagenes ?? Enumerable.Empty<IFormFile>())
        {
            if (formFile.Length == 0) continue;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            var fullPath = Path.Combine(carpeta, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await formFile.CopyToAsync(stream);

            _context.ImagenHabitacion.Add(new ImagenHabitacion
            {
                TipoHabitacionId = tipo.Id,
                Url = fileName,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            });
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }


    // POST: api/TiposHabitaciones/ConImagenes
    [HttpPost("ConImagenes")]
    public async Task<ActionResult<TipoHabitacionDTO>> PostTipoHabitacionConImagenes(
    [FromForm] TipoHabitacionConImagenesDTO dto)
    {
        Console.WriteLine($"Nombre: {dto.Nombre}");
        Console.WriteLine($"Descripcion: {dto.Descripcion}");
        Console.WriteLine($"Servicios.Count: {dto.Servicios?.Count}");
        Console.WriteLine($"Imagenes.Count: {dto.Imagenes?.Count}");
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState)
            {
                Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }

            return BadRequest(ModelState);
        }

        // 1️⃣  Creamos el TipoHabitacion
        var servicios = await _context.Servicio
            .Where(s => dto.Servicios.Contains(s.Id))
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
        await _context.SaveChangesAsync();   // ← obtenemos el Id

        // 2️⃣  Procesamos cada imagen
        var carpeta = Path.Combine(_environment.WebRootPath, "imagenes");
        Directory.CreateDirectory(carpeta);  // por si no existe

        foreach (var formFile in dto.Imagenes ?? Enumerable.Empty<IFormFile>())
        {
            if (formFile.Length == 0) continue;

            // nombre único
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            var fullPath = Path.Combine(carpeta, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // registramos en BD
            _context.ImagenHabitacion.Add(new ImagenHabitacion
            {
                TipoHabitacionId = tipo.Id,
                Url = fileName,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTipoHabitacion), new { id = tipo.Id }, ToDTO(tipo));
    }


    // DELETE: api/TiposHabitaciones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTipoHabitacion(int id)
    {
        var tipo = await _context.TipoHabitacion
        .Include(t => t.Habitaciones)
        .FirstOrDefaultAsync(t => t.Id == id);

        if (tipo == null)
            return NotFound();
        // Verificamos si hay habitaciones asociadas
        if (tipo.Habitaciones != null && tipo.Habitaciones.Any())
            return BadRequest("No se puede eliminar el tipo de habitación porque tiene habitaciones asociadas.");
        
        tipo.Activo = false;
        _context.Entry(tipo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool TipoHabitacionExists(int id) =>
        _context.TipoHabitacion.Any(e => e.Id == id);

    private TipoHabitacionDTO ToDTO(TipoHabitacion tipo)
    {
        // baseUrl:  https://miservidor.com  o  http://localhost:5000
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        return new TipoHabitacionDTO
        {
            Id = tipo.Id,
            Nombre = tipo.Nombre,
            Descripcion = tipo.Descripcion,
            PrecioBase = tipo.PrecioBase,
            CantidadDisponible = tipo.CantidadDisponible,
            MaximaOcupacion = tipo.MaximaOcupacion,
            Tamanho = tipo.Tamanho,

            Servicios = tipo.Servicios.Select(s => new ServicioDTO
            {
                Id = s.Id,
                Nombre = s.Nombre,
                IconName = s.IconName
            }).ToList(),

            Imagenes = tipo.ImagenesHabitaciones
                .Where(i => i.Activo)
                .Select(i => new ImagenHabitacionDTO
                {
                    Id = i.Id,
                    Url = $"{baseUrl}/imagenes/{i.Url}"
                })
                .ToList()
        };
    }


}

public class TipoHabitacionConImagenesDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioBase { get; set; }
    public int CantidadDisponible { get; set; }
    public int MaximaOcupacion { get; set; }
    public int Tamanho { get; set; }
    public List<int> Servicios { get; set; }
    public List<IFormFile>? Imagenes { get; set; }
}