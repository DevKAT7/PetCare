using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockTransactions.Queries
{
    public class GetStockTransactionsQuery : IRequest<List<StockTransactionReadModel>> { }

    public class GetStockTransactionsHandler : IRequestHandler<GetStockTransactionsQuery, List<StockTransactionReadModel>>
    {
        private readonly ApplicationDbContext _context;

        public GetStockTransactionsHandler(ApplicationDbContext context)
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
