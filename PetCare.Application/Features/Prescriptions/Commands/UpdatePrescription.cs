using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Prescriptions.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Prescriptions.Commands
{
    public class UpdatePrescriptionCommand : IRequest<int>
    {
        public int Id { get; }
        public PrescriptionCreateModel Prescription { get; set; }

        public UpdatePrescriptionCommand(int id, PrescriptionCreateModel model)
        {
            Id = id;
            Prescription = model;
        }
    }

    public class UpdatePrescriptionHandler : IRequestHandler<UpdatePrescriptionCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdatePrescriptionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.PrescriptionId == request.Id, cancellationToken);

            if (prescription == null)
            {
                throw new NotFoundException("Prescription", request.Id);
            }

            var model = request.Prescription;

            var appointmentExists = await _context.Appointments.AnyAsync(a => a.AppointmentId == model.AppointmentId, cancellationToken);

            if (!appointmentExists)
            {
                throw new NotFoundException("Appointment not found.");
            }

            var medicationExists = await _context.Medications.AnyAsync(m => m.MedicationId == model.MedicationId, cancellationToken);

            if (!medicationExists)
            {
                throw new NotFoundException("Medication not found.");
            }

            prescription.Dosage = model.Dosage;
            prescription.Frequency = model.Frequency;
            prescription.StartDate = model.StartDate;
            prescription.EndDate = model.EndDate;
            prescription.Instructions = model.Instructions;
            prescription.PacksToDispense = model.PacksToDispense;
            prescription.AppointmentId = model.AppointmentId;
            prescription.MedicationId = model.MedicationId;

            await _context.SaveChangesAsync(cancellationToken);

            return prescription.PrescriptionId;
        }
    }
}
