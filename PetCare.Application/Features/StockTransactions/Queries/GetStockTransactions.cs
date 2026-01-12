using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.StockTransactions.Queries
{
    public class GetStockTransactionsQuery : IRequest<List<StockTransactionReadModel>>
    {
        public int? MedicationId { get; set; }
    }

    public class GetStockTransactionsHandler : IRequestHandler<GetStockTransactionsQuery, List<StockTransactionReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetStockTransactionsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockTransactionReadModel>> Handle(GetStockTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.StockTransactions
            .Include(s => s.Medication)
            .AsQueryable();

            if (request.MedicationId.HasValue)
            {
                query = query.Where(t => t.MedicationId == request.MedicationId.Value);
            }

            query = query.OrderByDescending(t => t.Timestamp);

            return await query.Select(st => new StockTransactionReadModel
            {
                StockTransactionId = st.StockTransactionId,
                QuantityChange = st.QuantityChange,
                Timestamp = st.Timestamp,
                Reason = st.Reason,
                MedicationId = st.MedicationId,
                MedicationName = st.Medication != null ? st.Medication.Name : string.Empty
            })
            .ToListAsync(cancellationToken);
        }
    }
}
