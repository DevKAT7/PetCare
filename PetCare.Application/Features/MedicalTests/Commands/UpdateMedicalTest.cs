using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.MedicalTests.Dto;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.MedicalTests.Commands
{
    public class UpdateMedicalTestCommand : IRequest<int>
    {
        public int Id { get; }
        public MedicalTestCreateModel MedicalTest { get; set; }

        public UpdateMedicalTestCommand(int id, MedicalTestCreateModel model)
        {
            Id = id;
            MedicalTest = model;
        }
    }

    public class UpdateMedicalTestHandler : IRequestHandler<UpdateMedicalTestCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateMedicalTestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateMedicalTestCommand request, CancellationToken cancellationToken)
        {
            var medicalTest = await _context.MedicalTests
                .FirstOrDefaultAsync(m => m.MedicalTestId == request.Id, cancellationToken);

            if (medicalTest == null)
            {
                throw new NotFoundException("Medical test", request.Id);
            }

            var model = request.MedicalTest;

            medicalTest.TestName = model.TestName;
            medicalTest.Result = model.Result;
            medicalTest.TestDate = model.TestDate;
            medicalTest.AttachmentUrl = model.AttachmentUrl;
            medicalTest.PetId = model.PetId;
            medicalTest.AppointmentId = model.AppointmentId;

            await _context.SaveChangesAsync(cancellationToken);

            return medicalTest.MedicalTestId;
        }
    }
}
