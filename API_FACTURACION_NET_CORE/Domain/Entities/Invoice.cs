namespace API_FACTURACION_NET_CORE.Domain.Entities
{
    public class Invoice
    {
        public long Id { get; set; }

        public string ClaveAcceso { get; set; } = null!;

        public string Ruc { get; set; } = null!;

        public string CodDoc { get; set; } = null!;

        public string Estab { get; set; } = null!;

        public string PtoEmi { get; set; } = null!;

        public string Secuencial { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string PayloadJson { get; set; } = null!;

        public string? XmlPath { get; set; }

        public string? XmlContent { get; set; }

        public string? SriAuthorizationNumber { get; set; }

        public DateTime? SriAuthorizationDate { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}
