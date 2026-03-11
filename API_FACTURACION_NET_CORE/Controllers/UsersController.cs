using API_FACTURACION_NET_CORE.Application.DTOs.Users;
using API_FACTURACION_NET_CORE.Domain.Entities;
using API_FACTURACION_NET_CORE.Domain.Enums;
using API_FACTURACION_NET_CORE.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_FACTURACION_NET_CORE.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
        {
            if (request.Role == Roles.Client)
            {
                if (string.IsNullOrWhiteSpace(request.Identificacion)
                    || string.IsNullOrWhiteSpace(request.RazonSocial)
                    || string.IsNullOrWhiteSpace(request.NombreComercial)
                    || request.P12File is null
                    || string.IsNullOrWhiteSpace(request.P12Password))
                {
                    return BadRequest("Para usuarios Client son obligatorios: identificacion, razon social, nombre comercial, archivo .p12 y contraseña del .p12.");
                }

                if (!Path.GetExtension(request.P12File.FileName).Equals(".p12", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest("El archivo debe tener extensión .p12");
                }
            }

            string? p12FilePath = null;

            if (request.P12File is not null)
            {
                var solutionRoot = Directory.GetParent(_webHostEnvironment.ContentRootPath)?.FullName ?? _webHostEnvironment.ContentRootPath;
                var rawIdentificacion = request.Identificacion ?? "sin-identificacion";
                var identificacionFolder = string.Concat(rawIdentificacion.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
                if (string.IsNullOrWhiteSpace(identificacionFolder))
                {
                    identificacionFolder = "sin-identificacion";
                }

                var targetDirectory = Path.Combine(solutionRoot, "datosclientes", identificacionFolder);

                Directory.CreateDirectory(targetDirectory);

                var fileName = $"{Guid.NewGuid()}.p12";
                var fullPath = Path.Combine(targetDirectory, fileName);

                await using var stream = new FileStream(fullPath, FileMode.Create);
                await request.P12File.CopyToAsync(stream);

                p12FilePath = $"/datosclientes/{identificacionFolder}/{fileName}";
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                IsActive = request.IsActive,
                Identificacion = request.Identificacion,
                RazonSocial = request.RazonSocial,
                NombreComercial = request.NombreComercial,
                P12FilePath = p12FilePath,
                P12Password = request.P12Password
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users);
        }
    }
}
