using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Invoices.Queries
{
    public class GetAllInvoicesQuery : IRequest<List<InvoiceReadModel>>
    {
        public string? SearchTerm { get; set; }
        public string? FilterStatus { get; set; }
        public string SortColumn { get; set; } = "InvoiceDate";
        public string SortDirection { get; set; } = "desc";
    }

    public class GetAllInvoicesHandler : IRequestHandler<GetAllInvoicesQuery, List<InvoiceReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllInvoicesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceReadModel>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Invoices
                .Include(i => i.PetOwner)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(i =>
                    i.InvoiceNumber.ToLower().Contains(term) ||
                    (i.PetOwner.FirstName + " " + i.PetOwner.LastName).ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(request.FilterStatus))
            {
                if (request.FilterStatus == "Paid")
                {
                    query = query.Where(i => i.IsPaid);
                }
                else if (request.FilterStatus == "Unpaid")
                {
                    query = query.Where(i => !i.IsPaid);
                }
                else if (request.FilterStatus == "Overdue")
                {
                    var today = DateOnly.FromDateTime(DateTime.Now);
                    query = query.Where(i => !i.IsPaid && i.DueDate < today);
                }
            }

            query = request.SortColumn switch
            {
                "InvoiceNumber" => request.SortDirection == "asc" ? query.OrderBy(i => i.InvoiceNumber) : query.OrderByDescending(i => i.InvoiceNumber),
                "PetOwnerName" => request.SortDirection == "asc" ? query.OrderBy(i => i.PetOwner.LastName).ThenBy(i => i.PetOwner.FirstName) : query.OrderByDescending(i => i.PetOwner.LastName).ThenByDescending(i => i.PetOwner.FirstName),
                "TotalAmount" => request.SortDirection == "asc" ? query.OrderBy(i => i.TotalAmount) : query.OrderByDescending(i => i.TotalAmount),
                "DueDate" => request.SortDirection == "asc" ? query.OrderBy(i => i.DueDate) : query.OrderByDescending(i => i.DueDate),
                _ => request.SortDirection == "asc" ? query.OrderBy(i => i.InvoiceDate) : query.OrderByDescending(i => i.InvoiceDate),
            };

            var invoices = await query
                .Select(i => new InvoiceReadModel
                {
                    InvoiceId = i.InvoiceId,
                    InvoiceNumber = i.InvoiceNumber,
                    InvoiceDate = i.InvoiceDate.ToDateTime(TimeOnly.MinValue),
                    DueDate = i.DueDate.ToDateTime(TimeOnly.MinValue),
                    TotalAmount = i.TotalAmount,
                    IsPaid = i.IsPaid,
                    PaymentDate = i.PaymentDate.HasValue ? i.PaymentDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    PetOwnerId = i.PetOwnerId,
                    PetOwnerName = i.PetOwner.FirstName + " " + i.PetOwner.LastName,
                    AppointmentId = i.AppointmentId
                })
                .ToListAsync(cancellationToken);

            return invoices;
        }
    }
}
