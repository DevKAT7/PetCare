using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockItems.Commands
{
    public class DeleteStockItemCommand : IRequest<int>
    {
        public int Id { get; }

        public DeleteStockItemCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteStockItemHandler : IRequestHandler<DeleteStockItemCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public DeleteStockItemHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteStockItemCommand request, CancellationToken cancellationToken)
        {
            var stockItem = await _context.StockItems.FirstOrDefaultAsync(x => x.StockItemId == request.Id, cancellationToken);

            if (stockItem == null)
            {
                throw new NotFoundException("Stock item", request.Id);
            }

            _context.StockItems.Remove(stockItem);
            await _context.SaveChangesAsync(cancellationToken);

            return stockItem.MedicationId;
        }
    }
}
