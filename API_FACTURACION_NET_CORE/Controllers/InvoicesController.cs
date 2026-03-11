using API_FACTURACION_NET_CORE.Application.DTOs.Invoices;
using API_FACTURACION_NET_CORE.Application.Services.Invoices;
using API_FACTURACION_NET_CORE.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API_FACTURACION_NET_CORE.Controllers
{
    [ApiController]
    [Route("api/v1/invoices")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly AppDbContext _context;

        public InvoicesController(
            InvoiceService invoiceService,
            AppDbContext context)
        {
            _invoiceService = invoiceService;
            _context = context;
        }

        [HttpPost("issue")]
        public async Task<IActionResult> Issue([FromBody] InvoiceRequest request)
        {
            try
            {
                var result = await _invoiceService.ProcessInvoiceAsync(request);

                return Ok(new
                {
                    message = "Factura recibida correctamente.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("status/{claveAcceso}")]
        public async Task<IActionResult> GetStatus(string claveAcceso)
        {
            var invoice = await _context.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClaveAcceso == claveAcceso);

            if (invoice == null)
            {
                return NotFound(new
                {
                    message = "No se encontró la factura."
                });
            }

            return Ok(new
            {
                invoice.Id,
                invoice.ClaveAcceso,
                invoice.Ruc,
                invoice.Secuencial,
                invoice.Estado,
                invoice.ErrorMessage,
                invoice.CreatedAt,
                invoice.UpdatedAt,
                invoice.ProcessedAt
            });
        }

        [HttpGet("{claveAcceso:string}/xml")]
        public async Task<IActionResult> GetXml(string claveAcceso)
        {
            var invoice = await _context.Invoices
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ClaveAcceso == claveAcceso);

            if (invoice == null)
            {
                return NotFound(new
                {
                    message = "No se encontró la factura."
                });
            }

            if (string.IsNullOrWhiteSpace(invoice.XmlContent))
            {
                return BadRequest(new
                {
                    message = "La factura aún no tiene XML generado."
                });
            }

            return Content(invoice.XmlContent, "application/xml", Encoding.UTF8);
        }
    }
}
