using System.Text;
using System.Xml;
using Microsoft.Extensions.Options;

namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class SriClientService
    {
        private readonly HttpClient _httpClient;
        private readonly SriServiceSettings _settings;

        public SriClientService(HttpClient httpClient, IOptions<SriServiceSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<SriSubmissionResult> SendSignedInvoiceAsync(string signedXml, string claveAcceso, string ambiente, CancellationToken cancellationToken)
        {
            var urls = ResolveUrlsByAmbiente(ambiente);

            var recepcionResponse = await SendToRecepcionAsync(signedXml, urls.RecepcionUrl, cancellationToken);
            var estadoRecepcion = ExtractFirstNodeValue(recepcionResponse, "estado");

            if (!string.Equals(estadoRecepcion, "RECIBIDA", StringComparison.OrdinalIgnoreCase))
            {
                return new SriSubmissionResult
                {
                    EstadoRecepcion = estadoRecepcion ?? "DESCONOCIDO",
                    EstadoAutorizacion = "NO_EJECUTADA",
                    Mensaje = ExtractMessages(recepcionResponse)
                };
            }

            var autorizacionResponse = await SendToAutorizacionAsync(claveAcceso, urls.AutorizacionUrl, cancellationToken);

            var estadoAutorizacion = ExtractFirstNodeValue(autorizacionResponse, "estado") ?? "DESCONOCIDO";
            var numeroAutorizacion = ExtractFirstNodeValue(autorizacionResponse, "numeroAutorizacion");
            var fechaAutorizacionRaw = ExtractFirstNodeValue(autorizacionResponse, "fechaAutorizacion");

            DateTime? fechaAutorizacion = null;
            if (DateTime.TryParse(fechaAutorizacionRaw, out var parsedDate))
                fechaAutorizacion = parsedDate;

            return new SriSubmissionResult
            {
                EstadoRecepcion = estadoRecepcion ?? "DESCONOCIDO",
                EstadoAutorizacion = estadoAutorizacion,
                NumeroAutorizacion = numeroAutorizacion,
                FechaAutorizacion = fechaAutorizacion,
                Mensaje = ExtractMessages(autorizacionResponse)
            };
        }

        private async Task<string> SendToRecepcionAsync(string signedXml, string recepcionUrl, CancellationToken cancellationToken)
        {
            ValidateUrl(recepcionUrl, "RecepcionUrl");

            var comprobanteBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(signedXml));
            var soapBody = $"""
                           <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:ec="http://ec.gob.sri.ws.recepcion">
                              <soapenv:Header/>
                              <soapenv:Body>
                                 <ec:validarComprobante>
                                    <xml>{comprobanteBase64}</xml>
                                 </ec:validarComprobante>
                              </soapenv:Body>
                           </soapenv:Envelope>
                           """;

            return await PostSoapAsync(recepcionUrl, soapBody, cancellationToken);
        }

        private async Task<string> SendToAutorizacionAsync(string claveAcceso, string autorizacionUrl, CancellationToken cancellationToken)
        {
            ValidateUrl(autorizacionUrl, "AutorizacionUrl");

            var soapBody = $"""
                           <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:ec="http://ec.gob.sri.ws.autorizacion">
                              <soapenv:Header/>
                              <soapenv:Body>
                                 <ec:autorizacionComprobante>
                                    <claveAccesoComprobante>{claveAcceso}</claveAccesoComprobante>
                                 </ec:autorizacionComprobante>
                              </soapenv:Body>
                           </soapenv:Envelope>
                           """;

            return await PostSoapAsync(autorizacionUrl, soapBody, cancellationToken);
        }

        private (string RecepcionUrl, string AutorizacionUrl) ResolveUrlsByAmbiente(string? ambiente)
        {
            var ambienteNormalizado = ambiente?.Trim();

            if (string.Equals(ambienteNormalizado, "2", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ambienteNormalizado, "PRODUCCION", StringComparison.OrdinalIgnoreCase))
            {
                return (
                    SelectConfiguredUrl(_settings.RecepcionUrlProduccion, _settings.RecepcionUrl, "RecepcionUrlProduccion"),
                    SelectConfiguredUrl(_settings.AutorizacionUrlProduccion, _settings.AutorizacionUrl, "AutorizacionUrlProduccion")
                );
            }

            if (string.Equals(ambienteNormalizado, "1", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ambienteNormalizado, "PRUEBAS", StringComparison.OrdinalIgnoreCase))
            {
                return (
                    SelectConfiguredUrl(_settings.RecepcionUrlPruebas, _settings.RecepcionUrl, "RecepcionUrlPruebas"),
                    SelectConfiguredUrl(_settings.AutorizacionUrlPruebas, _settings.AutorizacionUrl, "AutorizacionUrlPruebas")
                );
            }

            throw new Exception("El ambiente del comprobante no es válido. Use '1'/'PRUEBAS' o '2'/'PRODUCCION'.");
        }

        private static string SelectConfiguredUrl(string? preferredUrl, string? fallbackUrl, string configName)
        {
            if (!string.IsNullOrWhiteSpace(preferredUrl))
                return preferredUrl;

            if (!string.IsNullOrWhiteSpace(fallbackUrl))
                return fallbackUrl;

            throw new Exception($"No se configuró SRI:{configName}.");
        }

        private async Task<string> PostSoapAsync(string url, string soapEnvelope, CancellationToken cancellationToken)
        {
            using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            using var response = await _httpClient.PostAsync(url, content, cancellationToken);

            var responseText = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error HTTP al consumir SRI ({response.StatusCode}). Body: {responseText}");

            return responseText;
        }

        private static string? ExtractFirstNodeValue(string xml, string localName)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var node = doc.SelectSingleNode($"//*[local-name()='{localName}']");
            return node?.InnerText?.Trim();
        }

        private static string? ExtractMessages(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var messageNodes = doc.SelectNodes("//*[local-name()='mensaje']");
            if (messageNodes is null || messageNodes.Count == 0)
                return null;

            var messages = new List<string>();
            foreach (XmlNode node in messageNodes)
            {
                var msg = node.InnerText?.Trim();
                if (!string.IsNullOrWhiteSpace(msg))
                    messages.Add(msg);
            }

            return messages.Count == 0 ? null : string.Join(" | ", messages);
        }

        private static void ValidateUrl(string? url, string configName)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new Exception($"No se configuró SRI:{configName}.");
        }
    }
}
