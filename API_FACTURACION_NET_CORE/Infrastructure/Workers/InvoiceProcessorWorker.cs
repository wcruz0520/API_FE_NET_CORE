using API_FACTURACION_NET_CORE.Application.Services.Invoices;
using API_FACTURACION_NET_CORE.Domain.Entities;
using API_FACTURACION_NET_CORE.Domain.Enums;
using API_FACTURACION_NET_CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API_FACTURACION_NET_CORE.Infrastructure.Workers
{
    public class InvoiceProcessorWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<InvoiceProcessorWorker> _logger;

        public InvoiceProcessorWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<InvoiceProcessorWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("InvoiceProcessorWorker iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingInvoicesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error general en InvoiceProcessorWorker.");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogInformation("InvoiceProcessorWorker detenido.");
        }

        private async Task ProcessPendingInvoicesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var validationService = scope.ServiceProvider.GetRequiredService<InvoiceValidationService>();
            var xmlGeneratorService = scope.ServiceProvider.GetRequiredService<InvoiceXmlGeneratorService>();

            var pendingInvoices = await context.Invoices
                .Where(x => x.Estado == InvoiceStatuses.Received)
                .OrderBy(x => x.CreatedAt)
                .Take(10)
                .ToListAsync(cancellationToken);

            if (pendingInvoices.Count == 0)
                return;

            foreach (var invoice in pendingInvoices)
            {
                try
                {
                    invoice.Estado = InvoiceStatuses.Processing;
                    invoice.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync(cancellationToken);

                    validationService.ValidatePayloadStructure(invoice.PayloadJson);

                    var xmlContent = xmlGeneratorService.GenerateXml(invoice.PayloadJson);

                    invoice.XmlContent = xmlContent;
                    invoice.Estado = InvoiceStatuses.XmlGenerated;
                    invoice.UpdatedAt = DateTime.UtcNow;

                    await context.SaveChangesAsync(cancellationToken);

                    await SimulateBusinessPipelineAsync(invoice, cancellationToken);

                    invoice.Estado = InvoiceStatuses.Validated;
                    invoice.ProcessedAt = DateTime.UtcNow;
                    invoice.UpdatedAt = DateTime.UtcNow;
                    invoice.ErrorMessage = null;

                    await context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Factura procesada. Id={InvoiceId}, ClaveAcceso={ClaveAcceso}",
                        invoice.Id,
                        invoice.ClaveAcceso);
                }
                catch (Exception ex)
                {
                    invoice.Estado = InvoiceStatuses.Error;
                    invoice.ErrorMessage = ex.Message;
                    invoice.UpdatedAt = DateTime.UtcNow;

                    await context.SaveChangesAsync(cancellationToken);

                    _logger.LogError(
                        ex,
                        "Error procesando factura. Id={InvoiceId}, ClaveAcceso={ClaveAcceso}",
                        invoice.Id,
                        invoice.ClaveAcceso);
                }
            }
        }

        private async Task SimulateBusinessPipelineAsync(Invoice invoice, CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            if (string.IsNullOrWhiteSpace(invoice.ClaveAcceso))
                throw new Exception("La clave de acceso es inválida.");
        }
    }
}
