using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Invoices.Queries
{
    public class GetInvoiceQuery : IRequest<InvoiceReadModel>
    {
        public int InvoiceId { get; }
        public GetInvoiceQuery(int invoiceId) => InvoiceId = invoiceId;
    }

    public class GetInvoiceHandler : IRequestHandler<GetInvoiceQuery, InvoiceReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetInvoiceHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceReadModel> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var inv = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.PetOwner)
                .FirstOrDefaultAsync(i => i.InvoiceId == request.InvoiceId, cancellationToken);

            if (inv == null)
            {
                throw new NotFoundException("Invoice", request.InvoiceId);
            }

            return new InvoiceReadModel
            {
                InvoiceId = inv.InvoiceId,
                InvoiceNumber = inv.InvoiceNumber,
                InvoiceDate = inv.InvoiceDate.ToDateTime(TimeOnly.MinValue),
                DueDate = inv.DueDate.ToDateTime(TimeOnly.MinValue),
                TotalAmount = inv.TotalAmount,
                IsPaid = inv.IsPaid,
                PaymentDate = inv.PaymentDate.HasValue ? inv.PaymentDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                PetOwnerId = inv.PetOwnerId,
                PetOwnerName = inv.PetOwner.FirstName + " " + inv.PetOwner.LastName,
                AppointmentId = inv.AppointmentId,
                Items = inv.InvoiceItems.Select(it => new InvoiceItemReadModel
                {
                    InvoiceItemId = it.InvoiceItemId,
                    Description = it.Description,
                    UnitPrice = it.UnitPrice,
                    Quantity = it.Quantity
                }).ToList()
            };
        }
    }
}
