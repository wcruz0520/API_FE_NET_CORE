namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class SriServiceSettings
    {
        public string RecepcionUrl { get; set; } = string.Empty;
        public string AutorizacionUrl { get; set; } = string.Empty;
        public string RecepcionUrlPruebas { get; set; } = string.Empty;
        public string AutorizacionUrlPruebas { get; set; } = string.Empty;
        public string RecepcionUrlProduccion { get; set; } = string.Empty;
        public string AutorizacionUrlProduccion { get; set; } = string.Empty;
        public string CertificatePath { get; set; } = string.Empty;
        public string CertificatePassword { get; set; } = string.Empty;
    }
}

