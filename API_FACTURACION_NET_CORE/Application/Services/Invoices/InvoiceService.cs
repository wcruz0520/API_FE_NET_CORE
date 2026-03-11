using System.Text.Json;
using API_FACTURACION_NET_CORE.Application.DTOs.Invoices;
using API_FACTURACION_NET_CORE.Domain.Entities;
using API_FACTURACION_NET_CORE.Domain.Enums;
using API_FACTURACION_NET_CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API_FACTURACION_NET_CORE.Application.Services.Invoices
{
    public class InvoiceService
    {
        private readonly AppDbContext _context;
        private readonly InvoiceValidationService _validationService;

        public InvoiceService(
            AppDbContext context,
            InvoiceValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<object> ProcessInvoiceAsync(InvoiceRequest request)
        {
            _validationService.ValidateRequest(request);

            var claveAcceso = request.InfoTributaria.ClaveAcceso.Trim();

            var exists = await _context.Invoices
                .AnyAsync(x => x.ClaveAcceso == claveAcceso);

            if (exists)
                throw new Exception("Ya existe una factura registrada con esa clave de acceso.");

            var payloadJson = JsonSerializer.Serialize(request);

            var invoice = new Invoice
            {
                ClaveAcceso = claveAcceso,
                Ruc = request.InfoTributaria.Ruc?.Trim() ?? string.Empty,
                CodDoc = request.InfoTributaria.CodDoc?.Trim() ?? string.Empty,
                Estab = request.InfoTributaria.Estab?.Trim() ?? string.Empty,
                PtoEmi = request.InfoTributaria.PtoEmi?.Trim() ?? string.Empty,
                Secuencial = request.InfoTributaria.Secuencial?.Trim() ?? string.Empty,
                Estado = InvoiceStatuses.Received,
                PayloadJson = payloadJson,
                CreatedAt = DateTime.UtcNow
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return new
            {
                invoice.Id,
                status = invoice.Estado,
                invoice.ClaveAcceso,
                invoice.Ruc,
                invoice.Secuencial,
                message = "Factura recibida y encolada para procesamiento."
            };
        }
    }
}
