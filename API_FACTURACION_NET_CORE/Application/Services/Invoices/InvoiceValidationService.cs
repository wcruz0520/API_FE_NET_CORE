using API_FACTURACION_NET_CORE.Application.DTOs.Invoices;
using System.Text.Json;

namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class InvoiceValidationService
    {
        public void ValidatePayloadStructure(string payloadJson)
        {
            if (string.IsNullOrWhiteSpace(payloadJson))
                throw new Exception("El payload de la factura está vacío.");

            var request = JsonSerializer.Deserialize<InvoiceRequest>(payloadJson);

            if (request == null)
                throw new Exception("No se pudo deserializar el payload de la factura.");

            ValidateRequest(request);
        }

        public void ValidateRequest(InvoiceRequest request)
        {
            if (request.InfoTributaria == null)
                throw new Exception("La sección infoTributaria es obligatoria.");

            if (request.InfoFactura == null)
                throw new Exception("La sección infoFactura es obligatoria.");

            if (request.Detalles == null || request.Detalles.Count == 0)
                throw new Exception("La factura debe contener al menos un detalle.");

            ValidateInfoTributaria(request.InfoTributaria);
            ValidateInfoFactura(request.InfoFactura);
            ValidateDetalles(request.Detalles);
        }

        private void ValidateInfoTributaria(InfoTributariaDto infoTributaria)
        {
            if (string.IsNullOrWhiteSpace(infoTributaria.Ambiente))
                throw new Exception("El ambiente es obligatorio.");

            var ambiente = infoTributaria.Ambiente.Trim();
            if (!string.Equals(ambiente, "1", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(ambiente, "2", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(ambiente, "PRUEBAS", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(ambiente, "PRODUCCION", StringComparison.OrdinalIgnoreCase))
                throw new Exception("El ambiente debe ser '1'/'PRUEBAS' o '2'/'PRODUCCION'.");

            if (string.IsNullOrWhiteSpace(infoTributaria.ClaveAcceso))
                throw new Exception("La claveAcceso es obligatoria.");

            if (infoTributaria.ClaveAcceso.Trim().Length != 49)
                throw new Exception("La claveAcceso debe tener 49 caracteres.");

            if (string.IsNullOrWhiteSpace(infoTributaria.Ruc))
                throw new Exception("El ruc es obligatorio.");

            if (infoTributaria.Ruc.Trim().Length != 13)
                throw new Exception("El ruc debe tener 13 caracteres.");

            if (string.IsNullOrWhiteSpace(infoTributaria.CodDoc))
                throw new Exception("El codDoc es obligatorio.");

            if (string.IsNullOrWhiteSpace(infoTributaria.Estab))
                throw new Exception("El estab es obligatorio.");

            if (string.IsNullOrWhiteSpace(infoTributaria.PtoEmi))
                throw new Exception("El ptoEmi es obligatorio.");

            if (string.IsNullOrWhiteSpace(infoTributaria.Secuencial))
                throw new Exception("El secuencial es obligatorio.");
        }

        private void ValidateInfoFactura(InfoFacturaDto infoFactura)
        {
            if (string.IsNullOrWhiteSpace(infoFactura.FechaEmision))
                throw new Exception("La fechaEmision es obligatoria.");

            if (string.IsNullOrWhiteSpace(infoFactura.ImporteTotal))
                throw new Exception("El importeTotal es obligatorio.");
        }

        private void ValidateDetalles(List<DetalleDto> detalles)
        {
            for (int i = 0; i < detalles.Count; i++)
            {
                var detalle = detalles[i];

                if (string.IsNullOrWhiteSpace(detalle.Descripcion))
                    throw new Exception($"La descripción del detalle en posición {i} es obligatoria.");

                if (detalle.Cantidad <= 0)
                    throw new Exception($"La cantidad del detalle en posición {i} debe ser mayor a cero.");
            }
        }
    }
}
