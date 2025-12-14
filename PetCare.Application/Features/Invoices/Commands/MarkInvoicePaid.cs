using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Invoices.Commands
{
    public class MarkInvoicePaidCommand : IRequest<int>
    {
        public int InvoiceId { get; }
        public DateTime PaymentDate { get; }

        public MarkInvoicePaidCommand(int invoiceId, DateTime paymentDate)
        {
            InvoiceId = invoiceId;
            PaymentDate = paymentDate;
        }
    }

    public class MarkInvoicePaidHandler : IRequestHandler<MarkInvoicePaidCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public MarkInvoicePaidHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(MarkInvoicePaidCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == request.InvoiceId, cancellationToken);
            if (invoice == null) throw new NotFoundException("Invoice", request.InvoiceId);

            invoice.IsPaid = true;
            invoice.PaymentDate = DateOnly.FromDateTime(request.PaymentDate);

            await _context.SaveChangesAsync(cancellationToken);

            return invoice.InvoiceId;
        }
    }
}
