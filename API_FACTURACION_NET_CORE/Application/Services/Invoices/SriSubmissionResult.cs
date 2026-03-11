namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class SriSubmissionResult
    {
        public string EstadoRecepcion { get; set; } = string.Empty;
        public string EstadoAutorizacion { get; set; } = string.Empty;
        public string? NumeroAutorizacion { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public string? Mensaje { get; set; }
    }
}
