using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.MedicalTests.Dtos;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;
using PetCare.Core.Models;

namespace PetCare.Application.Features.MedicalTests.Commands
{
    public class CreateMedicalTestCommand : IRequest<int>
    {
        public required MedicalTestCreateModel MedicalTest { get; set; }
    }

    public class CreateMedicalTestHandler : IRequestHandler<CreateMedicalTestCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateMedicalTestHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateMedicalTestCommand request, CancellationToken cancellationToken)
        {
            var model = request.MedicalTest;

            var pet = await _context.Pets
                .Include(p => p.PetOwner)
                .FirstOrDefaultAsync(p => p.PetId == model.PetId, cancellationToken);

            if (pet == null)
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

            if (pet.PetOwner.UserId != null)
            {
                await _mediator.Send(new CreateNotificationCommand
                {
                    Notification = new NotificationCreateModel
                    {
                        UserId = pet.PetOwner.UserId,
                        Type = NotificationType.MedicalTestResultReady,
                        Message = $"Medical test results ({model.TestName}) for {pet.Name} are now available."
                    }
                }, cancellationToken);
            }

            return medicalTest.MedicalTestId;
        }
    }
}
