using HotelApi.Data;
using HotelApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;
using System;

namespace HotelApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HotelApiContext _context;

        private readonly IConfiguration _config;

        public AuthController(HotelApiContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(RegisterDTO model)
        {
            if (_context.Usuario.Any(u => u.Username == model.Username))
            {
                return BadRequest("Usuario ya existe.");
            }

            // Hash de la contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Contrasenha);

            var user = new Usuario
            {
                Username = model.Username,
                Nombre = model.Nombre,
                HashContrasenha = passwordHash,
                Creacion = DateTime.Now,
                Actualizacion = DateTime.Now,
                Activo = true
            };

            _context.Usuario.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario creado con exito");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                var user = await _context.Usuario.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Contrasenha, user.HashContrasenha))
                {
                    return Unauthorized("Credenciales incorrectas.");
                }

                // actualizar ult sesion del usuario logeuado
                user.UltimoLogin = DateTime.Now;

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                var token = GenerateJwtToken(user.Username);
                return Ok(new { token, username = user.Username });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Login: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return StatusCode(500, "Error en el servidor.");
            }
        }

        /*
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Eliminar las cookies de sesión
            Response.Cookies.Delete("login_token");

            return Ok(new { message = "Logout exitoso" });
        }*/

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}