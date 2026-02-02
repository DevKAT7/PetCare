using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Invoices.Dtos;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Invoices.Commands
{
    public class CreateInvoiceCommand : IRequest<int>
    {
        public required InvoiceCreateModel Invoice { get; set; }
    }

    public class CreateInvoiceHandler : IRequestHandler<CreateInvoiceCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateInvoiceHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var model = request.Invoice;

            var owner = await _context.PetOwners.FindAsync(new object[] { model.PetOwnerId }, cancellationToken);
            if (owner == null) throw new Exceptions.NotFoundException("PetOwner", model.PetOwnerId);

            var appointment = await _context.Appointments.FindAsync(new object[] { model.AppointmentId }, cancellationToken);
            if (appointment == null) throw new Exceptions.NotFoundException("Appointment", model.AppointmentId);

            var invoice = new Invoice
            {
                InvoiceDate = DateOnly.FromDateTime(model.InvoiceDate),
                DueDate = DateOnly.FromDateTime(model.DueDate),
                PetOwnerId = model.PetOwnerId,
                AppointmentId = model.AppointmentId,
                TotalAmount = 0m,
                IsPaid = false
            };

            foreach (var it in model.Items.Where(x => x.Quantity > 0))
            {
                var item = new InvoiceItem
                {
                    Description = it.Description,
                    UnitPrice = it.UnitPrice,
                    Quantity = it.Quantity,
                    LineTotal = it.UnitPrice * it.Quantity
                };
                invoice.InvoiceItems.Add(item);
                invoice.TotalAmount += item.UnitPrice * item.Quantity;
            }

            // generate invoice number
            invoice.InvoiceNumber = $"FA-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync(cancellationToken);

            var petOwnerUserId = await _context.PetOwners
                .Where(p => p.PetOwnerId == invoice.PetOwnerId)
                .Select(p => p.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (petOwnerUserId != null)
            {
                await _mediator.Send(new CreateNotificationCommand
                {
                    Notification = new NotificationCreateModel
                    {
                        UserId = petOwnerUserId,
                        Type = NotificationType.InvoiceReady,
                        Message = $"New invoice #{invoice.InvoiceNumber} ({invoice.TotalAmount:C}) is available."
                    }
                }, cancellationToken);
            }

            return invoice.InvoiceId;
        }
    }
}
