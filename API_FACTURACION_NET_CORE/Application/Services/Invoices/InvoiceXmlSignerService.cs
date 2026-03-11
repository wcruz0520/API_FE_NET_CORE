using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class InvoiceXmlSignerService
    {
        public string SignXml(string xmlContent, string p12Path, string p12Password)
        {
            if (string.IsNullOrWhiteSpace(xmlContent))
                throw new Exception("No hay XML para firmar.");

            if (string.IsNullOrWhiteSpace(p12Path) || !File.Exists(p12Path))
                throw new Exception($"No se encontró el certificado .p12 en la ruta: {p12Path}");

            var cert = new X509Certificate2(
                p12Path,
                p12Password,
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.Exportable |
                X509KeyStorageFlags.PersistKeySet);

            var privateKey = cert.GetRSAPrivateKey();
            if (privateKey is null)
                throw new Exception("El certificado no contiene clave privada RSA.");

            var xmlDoc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            xmlDoc.LoadXml(xmlContent);

            var signedXml = new SignedXml(xmlDoc)
            {
                SigningKey = privateKey
            };

            var reference = new Reference
            {
                Uri = string.Empty
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());

            signedXml.AddReference(reference);
            signedXml.KeyInfo = new KeyInfo();
            signedXml.KeyInfo.AddClause(new KeyInfoX509Data(cert));

            signedXml.ComputeSignature();
            var xmlDigitalSignature = signedXml.GetXml();

            xmlDoc.DocumentElement?.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

            return xmlDoc.OuterXml;
        }
    }
}
