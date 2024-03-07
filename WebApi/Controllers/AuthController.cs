using BusinessLogic.Data;
using BusinessLogic.Logic;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApi.Dto;
using WebApi.Errors;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(GroupName = "APIAuth")]
    public class AuthController : BaseApiController
    {
        private readonly BanksDBContext _context;
        private readonly TokenService _tokenService;

        public AuthController(BanksDBContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        /// <summary>
        /// Login de usuario devolviendo un token JWT y los datos del usuario
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            // Buscar al usuario por el email
            var user = _context.Users.SingleOrDefault(u => u.Email == loginDto.Email);

            // Verificar si el usuario existe y la contraseña es correcta
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized(new CodeErrorResponse(401, "Credenciales Invalidas"));
            }

            // Generar el token JWT utilizando el TokenService
            var token = _tokenService.GenerateToken(user);

            // Crear un objeto anónimo para la respuesta o definir una clase si prefieres
            var response = new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Username,
                user.role,
                Token = token
            };

            // Retornar los datos del usuario y el token
            return Ok(response);
        }
       /// <summary>
       /// Crear un nuevo usuario y retornar los datos del usuario creado
       /// </summary>
       /// <param name="registerDto"></param>
       /// <returns></returns>
        [HttpPost("register")]
        [Authorize]
        public ActionResult<Users> Register([FromBody] RegisterDto registerDto)
        {
            // Verificar si el email ya existe
            if (_context.Users.Any(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email ya está en uso.");
            }

            // Hashear la contraseña
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            // Crear el usuario
            var user = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Username = registerDto.Username,
                role = "prueba",
                Password = hashedPassword // Guardar la contraseña hasheada
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Retorna el usuario creado (considera no retornar la contraseña, incluso hasheada, por seguridad)
            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Username
            });
        }
        /// <summary>
        /// Verificar si el token es válido o expiró
        /// </summary>
        /// <returns></returns>
        [HttpPost("verifyToken")]
        public ActionResult VerifyToken()
        {
            // Leer el token del header de autorización
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new CodeErrorResponse(400, "Token no proporcionado"));
            }

            // Verificar el token utilizando el TokenService
            var isValid = _tokenService.ValidateToken(token);

            if (!isValid)
            {
                return Unauthorized(new CodeErrorResponse(401, "Token inválido o expirado"));
            }

            return Ok(new { IsValid = true });
        }
    }
}
