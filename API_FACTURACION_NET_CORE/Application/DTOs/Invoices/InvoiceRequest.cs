using System.Text.Json.Serialization;

namespace API_FACTURACION_NET_CORE.Application.DTOs.Invoices
{
    public class InvoiceRequest
    {
        [JsonPropertyName("infoTributaria")]
        public required InfoTributariaDto InfoTributaria { get; set; }

        [JsonPropertyName("infoFactura")]
        public required InfoFacturaDto InfoFactura { get; set; }

        [JsonPropertyName("detalles")]
        public List<DetalleDto> Detalles { get; set; } = new();

        [JsonPropertyName("retenciones")]
        public List<RetencionDto> Retenciones { get; set; } = new();

        [JsonPropertyName("infoAdicional")]
        public List<InfoAdicionalDto> InfoAdicional { get; set; } = new();

        [JsonPropertyName("campoAdicional1")]
        public string? CampoAdicional1 { get; set; }

        [JsonPropertyName("campoAdicional2")]
        public string? CampoAdicional2 { get; set; }
    }
}
