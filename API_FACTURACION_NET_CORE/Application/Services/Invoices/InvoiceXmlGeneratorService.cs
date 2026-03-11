using API_FACTURACION_NET_CORE.Application.DTOs.Invoices;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class InvoiceXmlGeneratorService
    {
        public string GenerateXml(string payloadJson)
        {
            if (string.IsNullOrWhiteSpace(payloadJson))
                throw new Exception("El payloadJson está vacío.");

            var request = JsonSerializer.Deserialize<InvoiceRequest>(payloadJson);

            if (request == null)
                throw new Exception("No se pudo deserializar el payloadJson.");

            return GenerateXml(request);
        }

        public string GenerateXml(InvoiceRequest request)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using var stringWriter = new Utf8StringWriter();
            using var writer = XmlWriter.Create(stringWriter, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("factura");
            writer.WriteAttributeString("id", "comprobante");
            writer.WriteAttributeString("version", "1.0.0");

            WriteInfoTributaria(writer, request.InfoTributaria);
            WriteInfoFactura(writer, request.InfoFactura);
            WriteDetalles(writer, request.Detalles);
            WriteInfoAdicional(writer, request.InfoAdicional);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return stringWriter.ToString();
        }

        private void WriteInfoTributaria(XmlWriter writer, InfoTributariaDto dto)
        {
            writer.WriteStartElement("infoTributaria");

            WriteElement(writer, "ambiente", dto.Ambiente);
            WriteElement(writer, "tipoEmision", dto.TipoEmision);
            WriteElement(writer, "claveAcceso", dto.ClaveAcceso);
            WriteElement(writer, "razonSocial", dto.RazonSocial);
            WriteElement(writer, "nombreComercial", dto.NombreComercial);
            WriteElement(writer, "ruc", dto.Ruc);
            WriteElement(writer, "codDoc", dto.CodDoc);
            WriteElement(writer, "estab", dto.Estab);
            WriteElement(writer, "ptoEmi", dto.PtoEmi);
            WriteElement(writer, "secuencial", dto.Secuencial);
            WriteElement(writer, "dirMatriz", dto.DirMatriz);

            writer.WriteEndElement();
        }

        private void WriteInfoFactura(XmlWriter writer, InfoFacturaDto dto)
        {
            writer.WriteStartElement("infoFactura");

            WriteElement(writer, "fechaEmision", dto.FechaEmision);
            WriteElement(writer, "dirEstablecimiento", dto.DirEstablecimiento);
            WriteElement(writer, "contribuyenteEspecial", dto.ContribuyenteEspecial);
            WriteElement(writer, "obligadoContabilidad", dto.ObligadoContabilidad);
            WriteElement(writer, "tipoIdentificacionComprador", dto.TipoIdentificacionComprador);
            WriteElement(writer, "guiaRemision", dto.GuiaRemision);
            WriteElement(writer, "razonSocialComprador", dto.RazonSocialComprador);
            WriteElement(writer, "identificacionComprador", dto.IdentificacionComprador);
            WriteElement(writer, "direccionComprador", dto.DireccionComprador);
            WriteElement(writer, "totalSinImpuestos", dto.TotalSinImpuestos);
            WriteElement(writer, "totalDescuento", dto.TotalDescuento);

            if (dto.TotalConImpuestos != null && dto.TotalConImpuestos.Count > 0)
            {
                writer.WriteStartElement("totalConImpuestos");

                foreach (var item in dto.TotalConImpuestos)
                {
                    writer.WriteStartElement("totalImpuesto");
                    WriteElement(writer, "codigo", item.Codigo);
                    WriteElement(writer, "codigoPorcentaje", item.CodigoPorcentaje);
                    WriteElement(writer, "baseImponible", item.BaseImponible);
                    WriteElement(writer, "valor", item.Valor);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            WriteElement(writer, "propina", dto.Propina);
            WriteElement(writer, "importeTotal", dto.ImporteTotal);
            WriteElement(writer, "moneda", dto.Moneda);

            if (dto.Pagos != null && dto.Pagos.Count > 0)
            {
                writer.WriteStartElement("pagos");

                foreach (var pago in dto.Pagos)
                {
                    writer.WriteStartElement("pago");
                    WriteElement(writer, "formaPago", pago.FormaPago);
                    WriteElement(writer, "total", pago.Total);
                    WriteElement(writer, "plazo", pago.Plazo);
                    WriteElement(writer, "unidadTiempo", pago.UnidadTiempo);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteDetalles(XmlWriter writer, List<DetalleDto> detalles)
        {
            writer.WriteStartElement("detalles");

            foreach (var detalle in detalles)
            {
                writer.WriteStartElement("detalle");

                WriteElement(writer, "codigoPrincipal", detalle.CodigoPrincipal);
                WriteElement(writer, "codigoAuxiliar", detalle.CodigoAuxiliar);
                WriteElement(writer, "descripcion", detalle.Descripcion);
                WriteElement(writer, "cantidad", detalle.Cantidad.ToString("0.##"));
                WriteElement(writer, "precioUnitario", detalle.PrecioUnitario);
                WriteElement(writer, "descuento", detalle.Descuento);
                WriteElement(writer, "precioTotalSinImpuesto", detalle.PrecioTotalSinImpuesto);

                if (detalle.Impuestos != null && detalle.Impuestos.Count > 0)
                {
                    writer.WriteStartElement("impuestos");

                    foreach (var impuesto in detalle.Impuestos)
                    {
                        writer.WriteStartElement("impuesto");
                        WriteElement(writer, "codigo", impuesto.Codigo);
                        WriteElement(writer, "codigoPorcentaje", impuesto.CodigoPorcentaje);
                        WriteElement(writer, "tarifa", impuesto.Tarifa);
                        WriteElement(writer, "baseImponible", impuesto.BaseImponible);
                        WriteElement(writer, "valor", impuesto.Valor);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                if (detalle.DetallesAdicionales != null && detalle.DetallesAdicionales.Count > 0)
                {
                    writer.WriteStartElement("detallesAdicionales");

                    foreach (var adicional in detalle.DetallesAdicionales)
                    {
                        writer.WriteStartElement("detAdicional");
                        writer.WriteAttributeString("nombre", adicional.Nombre ?? string.Empty);
                        writer.WriteAttributeString("valor", adicional.Valor ?? string.Empty);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteInfoAdicional(XmlWriter writer, List<InfoAdicionalDto> infoAdicional)
        {
            if (infoAdicional == null || infoAdicional.Count == 0)
                return;

            writer.WriteStartElement("infoAdicional");

            foreach (var item in infoAdicional)
            {
                writer.WriteStartElement("campoAdicional");
                writer.WriteAttributeString("nombre", item.Nombre ?? string.Empty);
                writer.WriteString(item.Valor ?? string.Empty);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void WriteElement(XmlWriter writer, string elementName, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.WriteElementString(elementName, value);
            }
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
