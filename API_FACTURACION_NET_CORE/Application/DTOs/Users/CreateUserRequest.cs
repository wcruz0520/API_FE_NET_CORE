namespace API_FACTURACION_NET_CORE.Application.DTOs.Users
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public string? Identificacion { get; set; }

        public string? RazonSocial { get; set; }

        public string? NombreComercial { get; set; }

        public IFormFile? P12File { get; set; }

        public string? P12Password { get; set; }
    }

}
