using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockItems.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockItems.Commands
{
    public class CreateStockItemCommand : IRequest<int>
    {
        public required StockItemCreateModel StockItem { get; set; }
    }

    public class CreateStockItemHandler : IRequestHandler<CreateStockItemCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateStockItemHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateStockItemCommand request, CancellationToken cancellationToken)
        {
            var model = request.StockItem;

            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == model.MedicationId, cancellationToken);
            if (medication == null)
            {
                throw new NotFoundException("Medication", model.MedicationId);
            }

            var exists = await _context.StockItems.AnyAsync(si => si.MedicationId == model.MedicationId, cancellationToken);
            if (exists)
            {
                throw new BadRequestException("Stock item for this medication already exists.");
            }

            var entity = new StockItem
            {
                CurrentStock = model.CurrentStock,
                ReorderLevel = model.ReorderLevel,
                MedicationId = model.MedicationId
            };

            _context.StockItems.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.StockItemId;
        }
    }
}
