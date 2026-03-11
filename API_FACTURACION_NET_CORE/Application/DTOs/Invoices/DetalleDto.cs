using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_FACTURACION_NET_CORE.Application.DTOs.Invoices
{
    public class DetalleDto
    {
        [JsonPropertyName("codigoPrincipal")]
        public string? CodigoPrincipal { get; set; }

        [JsonPropertyName("codigoAuxiliar")]
        public string? CodigoAuxiliar { get; set; }

        [Required]
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = null!;

        [JsonPropertyName("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonPropertyName("precioUnitario")]
        public string? PrecioUnitario { get; set; }

        [JsonPropertyName("descuento")]
        public string? Descuento { get; set; }

        [JsonPropertyName("precioTotalSinImpuesto")]
        public string? PrecioTotalSinImpuesto { get; set; }

        [JsonPropertyName("detallesAdicionales")]
        public List<DetalleAdicionalDto> DetallesAdicionales { get; set; } = new();

        [JsonPropertyName("impuestos")]
        public List<ImpuestoDetalleDto> Impuestos { get; set; } = new();
    }
}
