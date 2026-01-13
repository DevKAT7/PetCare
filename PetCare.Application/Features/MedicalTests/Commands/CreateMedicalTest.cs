using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.MedicalTests.Commands
{
    public class CreateMedicalTestCommand : IRequest<int>
    {
        public required MedicalTestCreateModel MedicalTest { get; set; }
    }

    public class CreateMedicalTestHandler : IRequestHandler<CreateMedicalTestCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateMedicalTestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateMedicalTestCommand request, CancellationToken cancellationToken)
        {
            var model = request.MedicalTest;

            var petExists = await _context.Pets.AnyAsync(p => p.PetId == model.PetId, cancellationToken);

            if (!petExists)
            {
                throw new NotFoundException("Pet not found.");
            }

            var appointmentExists = await _context.Appointments.AnyAsync(a => a.AppointmentId == model.AppointmentId, cancellationToken);

            if (!appointmentExists)
            {
                throw new NotFoundException("Appointment not found.");
            }

            var medicalTest = new MedicalTest
            {
                TestName = model.TestName,
                Result = model.Result,
                TestDate = model.TestDate,
                AttachmentUrl = model.AttachmentUrl,
                PetId = model.PetId,
                AppointmentId = model.AppointmentId
            };

            _context.MedicalTests.Add(medicalTest);

            await _context.SaveChangesAsync(cancellationToken);

            return medicalTest.MedicalTestId;
        }
    }
}
