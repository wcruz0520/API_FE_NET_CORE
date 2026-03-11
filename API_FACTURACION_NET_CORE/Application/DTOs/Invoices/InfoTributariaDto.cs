using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_FACTURACION_NET_CORE.Application.DTOs.Invoices
{
    public class InfoTributariaDto
    {
        [Required]
        [JsonPropertyName("ambiente")]
        public string Ambiente { get; set; } = null!;

        [Required]
        [JsonPropertyName("tipoEmision")]
        public string TipoEmision { get; set; } = null!;

        [Required]
        [JsonPropertyName("claveAcceso")]
        public string ClaveAcceso { get; set; } = null!;

        [Required]
        [JsonPropertyName("razonSocial")]
        public string RazonSocial { get; set; } = null!;

        [JsonPropertyName("nombreComercial")]
        public string? NombreComercial { get; set; }

        [Required]
        [JsonPropertyName("ruc")]
        public string Ruc { get; set; } = null!;

        [Required]
        [JsonPropertyName("codDoc")]
        public string CodDoc { get; set; } = null!;

        [Required]
        [JsonPropertyName("estab")]
        public string Estab { get; set; } = null!;

        [Required]
        [JsonPropertyName("ptoEmi")]
        public string PtoEmi { get; set; } = null!;

        [Required]
        [JsonPropertyName("secuencial")]
        public string Secuencial { get; set; } = null!;

        [JsonPropertyName("dirMatriz")]
        public string? DirMatriz { get; set; }

        [JsonPropertyName("diaEmission")]
        public string? DiaEmission { get; set; }

        [JsonPropertyName("mesEmission")]
        public string? MesEmission { get; set; }

        [JsonPropertyName("anioEmission")]
        public string? AnioEmission { get; set; }
    }
}
