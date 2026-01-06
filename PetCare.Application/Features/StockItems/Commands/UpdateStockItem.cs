using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockItems.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockItems.Commands
{
    public class UpdateStockItemCommand : IRequest<int>
    {
        public int Id { get; }
        public StockItemUpdateModel StockItem { get; }

        public UpdateStockItemCommand(int id, StockItemUpdateModel stockItem)
        {
            Id = id;
            StockItem = stockItem;
        }
    }

    public class UpdateStockItemHandler : IRequestHandler<UpdateStockItemCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateStockItemHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateStockItemCommand request, CancellationToken cancellationToken)
        {
            var stockItem = await _context.StockItems.FirstOrDefaultAsync(x => x.StockItemId == request.Id, cancellationToken);

            if (stockItem == null)
            {
                throw new NotFoundException("Stock item not found.");
            }

            stockItem.CurrentStock = request.StockItem.CurrentStock;
            stockItem.ReorderLevel = request.StockItem.ReorderLevel;

            await _context.SaveChangesAsync(cancellationToken);

            return stockItem.StockItemId;
        }
    }
}
