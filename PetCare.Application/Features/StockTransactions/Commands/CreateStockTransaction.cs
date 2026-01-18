using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Features.StockTransactions.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;
using PetCare.Core.Models;

namespace PetCare.Application.Features.StockTransactions.Commands
{
    public class CreateStockTransactionCommand : IRequest<int>
    {
        public required StockTransactionCreateModel StockTransaction { get; set; }
    }

    public class CreateStockTransactionHandler : IRequestHandler<CreateStockTransactionCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public CreateStockTransactionHandler(IApplicationDbContext context, UserManager<User> userManager, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _mediator = mediator;
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
                throw new BadRequestException($"Insufficient storage quantity. " +
                    $"Current stock: {stockItem.CurrentStock - model.QuantityChange}, Attempted deduction: {Math.Abs(model.QuantityChange)}");
            }

            var transaction = new StockTransaction
            {
                QuantityChange = model.QuantityChange,
                Reason = model.Reason,
                MedicationId = model.MedicationId
            };

            if (stockItem.CurrentStock <= stockItem.ReorderLevel)
            {
                var vets = await _userManager.GetUsersInRoleAsync("Employee");
                var admins = await _userManager.GetUsersInRoleAsync("Admin");

                var recipientIds = vets
                    .Concat(admins)
                    .Select(u => u.Id)
                    .Distinct()
                    .ToList();

                foreach (var userId in recipientIds)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        Notification = new NotificationCreateModel
                        {
                            UserId = userId,
                            Type = NotificationType.LowStock,
                            Message = $"Attention! Low stock levels for this product: {medication.Name}. Remains: {stockItem.CurrentStock}."
                        }
                    }, cancellationToken);
                }
            }

            _context.StockTransactions.Add(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            return transaction.StockTransactionId;
        }
    }
}
