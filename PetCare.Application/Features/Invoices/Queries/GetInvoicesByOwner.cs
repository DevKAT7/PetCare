using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Invoices.Queries
{
    public class GetInvoicesByOwnerQuery : IRequest<List<InvoiceReadModel>>
    {
        public int PetOwnerId { get; }
        public GetInvoicesByOwnerQuery(int petOwnerId) => PetOwnerId = petOwnerId;
    }

    public class GetInvoicesByOwnerHandler : IRequestHandler<GetInvoicesByOwnerQuery, List<InvoiceReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetInvoicesByOwnerHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceReadModel>> Handle(GetInvoicesByOwnerQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.PetOwner)
                .Where(i => i.PetOwnerId == request.PetOwnerId)
                .Select(inv => new InvoiceReadModel
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
                }).ToListAsync(cancellationToken);

            return list;
        }
    }
}
