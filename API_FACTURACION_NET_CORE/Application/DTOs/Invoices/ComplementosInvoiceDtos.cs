using System.Text.Json.Serialization;

namespace API_FACTURACION_NET_CORE.Application.DTOs.Invoices
{
    public class TotalImpuestoDto
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("codigoPorcentaje")]
        public string? CodigoPorcentaje { get; set; }

        [JsonPropertyName("baseImponible")]
        public string? BaseImponible { get; set; }

        [JsonPropertyName("valor")]
        public string? Valor { get; set; }
    }

    public class PagoDto
    {
        [JsonPropertyName("formaPago")]
        public string? FormaPago { get; set; }

        [JsonPropertyName("total")]
        public string? Total { get; set; }

        [JsonPropertyName("plazo")]
        public string? Plazo { get; set; }

        [JsonPropertyName("unidadTiempo")]
        public string? UnidadTiempo { get; set; }
    }

    public class ReembolsoDto
    {
        [JsonPropertyName("tipoIdentificacionProveedorReembolso")]
        public string? TipoIdentificacionProveedorReembolso { get; set; }

        [JsonPropertyName("identificacionProveedorReembolso")]
        public string? IdentificacionProveedorReembolso { get; set; }

        [JsonPropertyName("codPaisPagoProveedorReembolso")]
        public string? CodPaisPagoProveedorReembolso { get; set; }

        [JsonPropertyName("tipoProveedorReembolso")]
        public string? TipoProveedorReembolso { get; set; }

        [JsonPropertyName("codDocReembolso")]
        public string? CodDocReembolso { get; set; }

        [JsonPropertyName("estabDocReembolso")]
        public string? EstabDocReembolso { get; set; }

        [JsonPropertyName("ptoEmiDocReembolso")]
        public string? PtoEmiDocReembolso { get; set; }

        [JsonPropertyName("secuencialDocReembolso")]
        public string? SecuencialDocReembolso { get; set; }

        [JsonPropertyName("fechaEmisionDocReembolso")]
        public string? FechaEmisionDocReembolso { get; set; }

        [JsonPropertyName("numeroautorizacionDocReemb")]
        public string? NumeroautorizacionDocReemb { get; set; }

        [JsonPropertyName("detalleImpuestos")]
        public List<DetalleImpuestoReembolsoDto> DetalleImpuestos { get; set; } = new();
    }

    public class DetalleImpuestoReembolsoDto
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("codigoPorcentaje")]
        public string? CodigoPorcentaje { get; set; }

        [JsonPropertyName("baseImponibleReembolso")]
        public string? BaseImponibleReembolso { get; set; }

        [JsonPropertyName("tarifa")]
        public string? Tarifa { get; set; }

        [JsonPropertyName("impuestoReembolso")]
        public string? ImpuestoReembolso { get; set; }
    }

    public class DetalleAdicionalDto
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("valor")]
        public string? Valor { get; set; }
    }

    public class ImpuestoDetalleDto
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("codigoPorcentaje")]
        public string? CodigoPorcentaje { get; set; }

        [JsonPropertyName("baseImponible")]
        public string? BaseImponible { get; set; }

        [JsonPropertyName("valor")]
        public string? Valor { get; set; }

        [JsonPropertyName("tarifa")]
        public string? Tarifa { get; set; }
    }

    public class RetencionDto
    {
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("codigoPorcentaje")]
        public string? CodigoPorcentaje { get; set; }

        [JsonPropertyName("tarifa")]
        public string? Tarifa { get; set; }

        [JsonPropertyName("valor")]
        public string? Valor { get; set; }
    }

    public class InfoAdicionalDto
    {
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("valor")]
        public string? Valor { get; set; }
    }
}
