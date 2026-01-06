using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.StockTransactions.Commands
{
    public class CreateStockTransactionCommand : IRequest<int>
    {
        public required StockTransactionCreateModel StockTransaction { get; set; }
    }

    public class CreateStockTransactionHandler : IRequestHandler<CreateStockTransactionCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateStockTransactionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateStockTransactionCommand request, CancellationToken cancellationToken)
        {
            var model = request.StockTransaction;

            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == model.MedicationId, cancellationToken);
            if (medication == null)
            {
                throw new NotFoundException("Medication", model.MedicationId);
            }

            var stockItem = await _context.StockItems
            .FirstOrDefaultAsync(si => si.MedicationId == model.MedicationId, cancellationToken);

            if (stockItem == null)
            {
                stockItem = new StockItem
                {
                    MedicationId = model.MedicationId,
                    CurrentStock = 0,
                    ReorderLevel = 5
                };
                _context.StockItems.Add(stockItem);
            }

            stockItem.CurrentStock += model.QuantityChange;

            if (stockItem.CurrentStock < 0)
            {
                throw new BadRequestException($"Niewystarczaj¹ca iloœæ towaru w magazynie. " +
                    $"Obecny stan: {stockItem.CurrentStock - model.QuantityChange}, Próba odjêcia: {Math.Abs(model.QuantityChange)}");
            }

            var transaction = new StockTransaction
            {
                QuantityChange = model.QuantityChange,
                Reason = model.Reason,
                MedicationId = model.MedicationId
            };

            _context.StockTransactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            return transaction.StockTransactionId;
        }
    }
}
