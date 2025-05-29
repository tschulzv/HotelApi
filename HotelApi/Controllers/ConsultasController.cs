using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApi.Data;
using HotelApi.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using HotelApi.DTOs;
using HotelApi.DTOs.Request;
using System.Net.Mail;
using System.Net;


namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly HotelApiContext _context;

        public ConsultasController(HotelApiContext context)
        {
            _context = context;
        }

        // GET: api/Consultas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaDTO>>> GetConsulta()
        {
            var consultas = await _context.Consulta.Where(c => c.Activo).ToListAsync();
            return consultas.Select(ToDTO).ToList();
        }

        // GET: api/Consultas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaDTO>> GetConsulta(int id)
        {
           
            var consulta = await _context.Consulta.Where(c => c.Activo && c.Id == id).FirstOrDefaultAsync();

            if (consulta == null)
            {
                return NotFound();
            }

            return ToDTO(consulta);
        }

        // POST: api/Consultas
        [HttpPost]
        public async Task<ActionResult<ConsultaDTO>> PostConsulta(ConsultaDTO consultaDTO)
        {
            var consulta = new Consulta
            {
                Nombre = consultaDTO.Nombre,
                Email = consultaDTO.Email,
                Telefono = consultaDTO.Telefono,
                Mensaje = consultaDTO.Mensaje,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = consultaDTO.Activo
            };
            _context.Consulta.Add(consulta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, ToDTO(consulta));
        }


        // POST: api/Consultas/public ; crea la solicitud asociada
        [HttpPost("public")]
        public async Task<ActionResult<ConsultaDTO>> PostPublicConsulta(ConsultaDTO consultaDTO)
        {
            var consulta = new Consulta
            {
                Nombre = consultaDTO.Nombre,
                Email = consultaDTO.Email,
                Telefono = consultaDTO.Telefono,
                Mensaje = consultaDTO.Mensaje,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.Consulta.Add(consulta);

            var solicitud = new Solicitud
            {
                ConsultaId = consulta.Id,
                Consulta = consulta,
                Tipo = "Consulta",
                EsLeida = false,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };
            _context.Solicitud.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, ToDTO(consulta));
        }

        // PUT: api/Consultas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta(int id, ConsultaDTO consultaDTO)
        {
            if (id != consultaDTO.Id)
            {
                return BadRequest();
            }

            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            consulta.Nombre = consultaDTO.Nombre;
            consulta.Email = consultaDTO.Email;
            consulta.Telefono = consultaDTO.Telefono;
            consulta.Mensaje = consultaDTO.Mensaje;
            consulta.Actualizacion = DateTime.Now;
            consulta.Activo = consultaDTO.Activo;

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        // DELETE: api/Consultas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null || !consulta.Activo)
            {
                return NotFound();
            }

            consulta.Activo = false;
            consulta.Actualizacion = DateTime.Now;

            _context.Entry(consulta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(id))
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

        [HttpPost("answer")]
        public async Task<IActionResult> ResponderConsulta(ResponderConsultaRequest req)
        {
            var consulta = await _context.Consulta.FindAsync(req.ConsultaId);

            if (consulta == null || string.IsNullOrWhiteSpace(consulta.Email))
            {
                return BadRequest("Consulta no encontrada o sin email.");
            }

            // Enviar email primero, si falla no se hace nada en la base de datos
            try
            {
                EnviarEmail(consulta.Nombre, consulta.Email, consulta.Mensaje, req.Texto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el email: {ex.Message}");
                return StatusCode(500, "No se pudo enviar el email. La consulta no fue marcada como contestada.");
            }

            // Email enviado con éxito, proceder a actualizar la base de datos dentro de una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                consulta.EsContestada = true;
                consulta.Actualizacion = DateTime.Now;
                _context.Entry(consulta).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al actualizar la base de datos: {ex.Message}");
                return StatusCode(500, "Error interno al guardar cambios.");
            }
        }


        private static void EnviarEmail(string nombreCliente, string emailDestino, string consulta, string respuesta)
        {
            try
            {
                var fromAddress = new MailAddress("hotellosalamospy@gmail.com", "Hotel Los Alamos");

                var toAddress = new MailAddress(emailDestino);
                const string fromPassword = "qnacddvmoiwxpfkl";

                string subject = "Confirmación de Reserva";

                string body = GenerarCuerpoCorreo(nombreCliente, consulta, respuesta);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enviando email: " + ex.Message);
            }
        }


        // correo de respuesta a consulta
        private static string GenerarCuerpoCorreo(string nombreCliente, string consulta, string respuesta)
        {
            return $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333;'>
                <h2 style='color: #2c3e50;'>Respuesta a su consulta</h2>
                <p>Estimado/a <strong>{nombreCliente}</strong>,</p>

                <p>Gracias por contactarse con <strong>Hotel Los Álamos</strong>. A continuación le enviamos una respuesta a su consulta.</p>

                <p><strong>Su consulta:</strong></p>
                <div style='border: 1px solid #ccc; padding: 15px; background-color: #f0f0f0; margin: 10px 0; white-space: pre-wrap;'>
                    {System.Net.WebUtility.HtmlEncode(consulta)}
                </div>

                <p><strong>Nuestra respuesta:</strong></p>
                <div style='border: 1px solid #ccc; padding: 15px; background-color: #f9f9f9; margin: 10px 0; white-space: pre-wrap;'>
                    {System.Net.WebUtility.HtmlEncode(respuesta)}
                </div>

                <p>Si tiene más dudas o desea información adicional, no dude en escribirnos.</p>

                <p>Saludos cordiales,<br><strong>Hotel Los Álamos</strong></p>
            </body>
            </html>";
        }



        private bool ConsultaExists(int id)
        {
            return _context.Consulta.Any(e => e.Id == id);
        }
        public static ConsultaDTO ToDTO(Consulta consulta)
        {
            return new ConsultaDTO
            {
                Id = consulta.Id,
                Nombre = consulta.Nombre,
                Email = consulta.Email,
                Telefono = consulta.Telefono,
                Mensaje = consulta.Mensaje,
                EsContestada = consulta.EsContestada,
                Activo = consulta.Activo
            };
        }
    }
}
