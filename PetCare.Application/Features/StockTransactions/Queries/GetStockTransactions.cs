using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.StockTransactions.Queries
{
    public class GetStockTransactionsQuery : IRequest<List<StockTransactionReadModel>> { }

    public class GetStockTransactionsHandler : IRequestHandler<GetStockTransactionsQuery, List<StockTransactionReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetStockTransactionsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StockTransactionReadModel>> Handle(GetStockTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.StockTransactions
                .Include(s => s.Medication)
                .Select(st => new StockTransactionReadModel
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
