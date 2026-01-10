using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.MedicalTests.Dto;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.MedicalTests.Queries
{
    public class GetMedicalTestQuery : IRequest<MedicalTestReadModel>
    {
        public int MedicalTestId { get; set; }
    }

    public class GetMedicalTestHandler : IRequestHandler<GetMedicalTestQuery, MedicalTestReadModel>
    {
        private readonly IApplicationDbContext _context;

        public GetMedicalTestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MedicalTestReadModel> Handle(GetMedicalTestQuery request, CancellationToken cancellationToken)
        {
            var medicalTest = await _context.MedicalTests
                .FirstOrDefaultAsync(m => m.MedicalTestId == request.MedicalTestId, cancellationToken);

            if (medicalTest == null)
            {
                throw new NotFoundException("Medical test not found.");
            }

            return new MedicalTestReadModel
            {
                MedicalTestId = medicalTest.MedicalTestId,
                TestName = medicalTest.TestName,
                Result = medicalTest.Result,
                TestDate = medicalTest.TestDate,
                AttachmentUrl = medicalTest.AttachmentUrl,
                PetId = medicalTest.PetId,
                AppointmentId = medicalTest.AppointmentId
            };
        }
    }
}
