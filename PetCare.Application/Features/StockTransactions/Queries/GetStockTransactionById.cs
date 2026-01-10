using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.StockTransactions.Queries
{
    public class GetStockTransactionQuery : IRequest<StockTransactionReadModel>
    {
        public int Id { get; set; }
    }

    public class GetStockTransactionHandler : IRequestHandler<GetStockTransactionQuery, StockTransactionReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetStockTransactionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockTransactionReadModel> Handle(GetStockTransactionQuery request, CancellationToken cancellationToken)
        {
            var stockTransaction = await _context.StockTransactions
                .Include(s => s.Medication)
                .FirstOrDefaultAsync(x => x.StockTransactionId == request.Id, cancellationToken);

            if (stockTransaction == null)
            {
                throw new NotFoundException("Stock transaction", request.Id);
            }

            return new StockTransactionReadModel
            {
                StockTransactionId = stockTransaction.StockTransactionId,
                QuantityChange = stockTransaction.QuantityChange,
                Timestamp = stockTransaction.Timestamp,
                Reason = stockTransaction.Reason,
                MedicationId = stockTransaction.MedicationId,
                MedicationName = stockTransaction.Medication?.Name ?? string.Empty
            };
        }
    }
}
