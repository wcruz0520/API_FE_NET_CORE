using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_FACTURACION_NET_CORE.Application.DTOs.Invoices
{
    public class InfoFacturaDto
    {
        [Required]
        [JsonPropertyName("fechaEmision")]
        public string FechaEmision { get; set; } = null!;

        [JsonPropertyName("dirEstablecimiento")]
        public string? DirEstablecimiento { get; set; }

        [JsonPropertyName("contribuyenteEspecial")]
        public string? ContribuyenteEspecial { get; set; }

        [JsonPropertyName("obligadoContabilidad")]
        public string? ObligadoContabilidad { get; set; }

        [JsonPropertyName("tipoIdentificacionComprador")]
        public string? TipoIdentificacionComprador { get; set; }

        [JsonPropertyName("guiaRemision")]
        public string? GuiaRemision { get; set; }

        [JsonPropertyName("razonSocialComprador")]
        public string? RazonSocialComprador { get; set; }

        [JsonPropertyName("identificacionComprador")]
        public string? IdentificacionComprador { get; set; }

        [JsonPropertyName("direccionComprador")]
        public string? DireccionComprador { get; set; }

        [JsonPropertyName("totalSinImpuestos")]
        public string? TotalSinImpuestos { get; set; }

        [JsonPropertyName("totalDescuento")]
        public string? TotalDescuento { get; set; }

        [JsonPropertyName("totalConImpuestos")]
        public List<TotalImpuestoDto> TotalConImpuestos { get; set; } = new();

        [JsonPropertyName("propina")]
        public string? Propina { get; set; }

        [JsonPropertyName("importeTotal")]
        public string? ImporteTotal { get; set; }

        [JsonPropertyName("moneda")]
        public string? Moneda { get; set; }

        [JsonPropertyName("pagos")]
        public List<PagoDto> Pagos { get; set; } = new();

        [JsonPropertyName("valorRetIva")]
        public string? ValorRetIva { get; set; }

        [JsonPropertyName("valorRetRenta")]
        public string? ValorRetRenta { get; set; }

        [JsonPropertyName("comercioExterior")]
        public string? ComercioExterior { get; set; }

        [JsonPropertyName("IncoTermFactura")]
        public string? IncoTermFactura { get; set; }

        [JsonPropertyName("lugarIncoTerm")]
        public string? LugarIncoTerm { get; set; }

        [JsonPropertyName("paisOrigen")]
        public string? PaisOrigen { get; set; }

        [JsonPropertyName("puertoEmbarque")]
        public string? PuertoEmbarque { get; set; }

        [JsonPropertyName("paisDestino")]
        public string? PaisDestino { get; set; }

        [JsonPropertyName("paisAdquisicion")]
        public string? PaisAdquisicion { get; set; }

        [JsonPropertyName("incoTermTotalSinImpuestos")]
        public string? IncoTermTotalSinImpuestos { get; set; }

        [JsonPropertyName("fleteInternacional")]
        public string? FleteInternacional { get; set; }

        [JsonPropertyName("seguroInternacional")]
        public string? SeguroInternacional { get; set; }

        [JsonPropertyName("gastosAduaneros")]
        public string? GastosAduaneros { get; set; }

        [JsonPropertyName("gastosTransporteOtros")]
        public string? GastosTransporteOtros { get; set; }

        [JsonPropertyName("codDocReembolso")]
        public string? CodDocReembolso { get; set; }

        [JsonPropertyName("totalComprobantesReembolso")]
        public string? TotalComprobantesReembolso { get; set; }

        [JsonPropertyName("totalBaseImponibleReembolso")]
        public string? TotalBaseImponibleReembolso { get; set; }

        [JsonPropertyName("totalImpuestoReembolso")]
        public string? TotalImpuestoReembolso { get; set; }

        [JsonPropertyName("reembolsos")]
        public List<ReembolsoDto> Reembolsos { get; set; } = new();
    }
}
