using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockTransactions.Commands
{
    public class DeleteStockTransactionCommand : IRequest<int>
    {
        public int Id { get; }

        public DeleteStockTransactionCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteStockTransactionHandler : IRequestHandler<DeleteStockTransactionCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public DeleteStockTransactionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteStockTransactionCommand request, CancellationToken cancellationToken)
        {
            var stockTransaction = await _context.StockTransactions.FirstOrDefaultAsync(x => x.StockTransactionId == request.Id, cancellationToken);

            if (stockTransaction == null)
            {
                throw new NotFoundException("Stock transaction", request.Id);
            }

            _context.StockTransactions.Remove(stockTransaction);

            await _context.SaveChangesAsync(cancellationToken);

            return stockTransaction.StockTransactionId;
        }
    }
}
