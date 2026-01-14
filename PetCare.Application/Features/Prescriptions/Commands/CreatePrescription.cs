using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.Prescriptions.Commands
{
    public class CreatePrescriptionCommand : IRequest<int>
    {
        public required PrescriptionCreateModel Prescription { get; set; }
    }

    public class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePrescriptionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var model = request.Prescription;

            var appointmentExists = await _context.Appointments.AnyAsync(a => a.AppointmentId == model.AppointmentId, cancellationToken);

            if (!appointmentExists)
            {
                throw new NotFoundException("Appointment not found.");
            }

            var stockItem = await _context.StockItems
                .Include(s => s.Medication)
                .FirstOrDefaultAsync(s => s.MedicationId == model.MedicationId, cancellationToken);

            if (stockItem == null)
            {
                var medExists = await _context.Medications.AnyAsync(m => m.MedicationId == model.MedicationId, cancellationToken);
                if (!medExists)
                {
                    throw new NotFoundException("Medication not found.");
                }

                throw new InvalidOperationException($"No stock record for medication. Cannot dispense.");
            }

            if (stockItem.CurrentStock < model.PacksToDispense)
            {
                throw new InvalidOperationException($"Not enough stock for '{stockItem.Medication.Name}'. " +
                    $"Available: {stockItem.CurrentStock}, Requested: {model.PacksToDispense}");
            }

            stockItem.CurrentStock -= model.PacksToDispense;

            var transaction = new StockTransaction
            {
                MedicationId = model.MedicationId,
                QuantityChange = -model.PacksToDispense,
                Reason = $"Prescription for Appointment #{model.AppointmentId}",
                Timestamp = DateTime.Now
            };

            _context.StockTransactions.Add(transaction);

            var prescription = new Prescription
            {
                Dosage = model.Dosage,
                Frequency = model.Frequency,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Instructions = model.Instructions,
                PacksToDispense = model.PacksToDispense,
                CreatedDate = DateOnly.FromDateTime(DateTime.Today),
                AppointmentId = model.AppointmentId,
                MedicationId = model.MedicationId
            };

            _context.Prescriptions.Add(prescription);

            await _context.SaveChangesAsync(cancellationToken);

            return prescription.PrescriptionId;
        }
    }
}
