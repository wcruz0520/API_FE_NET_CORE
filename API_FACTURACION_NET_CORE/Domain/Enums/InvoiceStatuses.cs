namespace API_FACTURACION_NET_CORE.Domain.Enums
{
    public static class InvoiceStatuses
    {
        public const string Received = "RECEIVED";
        public const string Processing = "PROCESSING";
        public const string Validated = "VALIDATED";
        public const string Authorized = "AUTHORIZED";
        public const string Error = "ERROR";
        public const string XmlGenerated = "XML_GENERATED";
    }
}
