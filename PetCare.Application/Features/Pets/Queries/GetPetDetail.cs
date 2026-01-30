using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Shared.Dtos;

namespace PetCare.Application.Features.Pets.Queries
{
    public class GetPetDetailQuery : IRequest<PetDetailDto>
    {
        public int PetId { get; set; }
    }

    public class GetPetDetailHandler : IRequestHandler<GetPetDetailQuery, PetDetailDto>
    {
        private readonly IApplicationDbContext _context;

        public GetPetDetailHandler(IApplicationDbContext context) => _context = context;

        public async Task<PetDetailDto> Handle(GetPetDetailQuery request, CancellationToken cancellationToken)
        {
            var pet = await _context.Pets
                .Include(p => p.PetOwner).ThenInclude(o => o.User)
                .Include(p => p.Appointments).ThenInclude(a => a.Vet)
                .Include(p => p.Appointments).ThenInclude(a => a.Prescriptions).ThenInclude(pr => pr.Medication)
                .Include(p => p.MedicalTests)
                .Include(p => p.Vaccinations)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.PetId == request.PetId, cancellationToken);

            if (pet == null) throw new NotFoundException("Pet", request.PetId);

            var dto = new PetDetailDto
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                DateOfBirth = pet.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                IsMale = pet.IsMale,
                ImageUrl = pet.ImageUrl,
                PetOwnerId = pet.PetOwnerId,
                OwnerFullName = $"{pet.PetOwner.FirstName} {pet.PetOwner.LastName}",
                OwnerPhoneNumber = pet.PetOwner.PhoneNumber,
                OwnerEmail = pet.PetOwner.User?.Email ?? "No email",

                Appointments = pet.Appointments.OrderByDescending(a => a.AppointmentDateTime).Select(a => new PetAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    Date = a.AppointmentDateTime,
                    Diagnosis = a.Diagnosis,
                    VetName = a.Vet != null ? $"{a.Vet.FirstName} {a.Vet.LastName}" : "Unknown",
                    Status = a.Status.ToString()
                }).ToList(),

                MedicalTests = pet.MedicalTests.OrderByDescending(t => t.TestDate).Select(t => new PetTestDto
                {
                    MedicalTestId = t.MedicalTestId,
                    Date = t.TestDate,
                    TestName = t.TestName,
                    Result = t.Result
                }).ToList(),

                Vaccinations = pet.Vaccinations.OrderByDescending(v => v.VaccinationDate).Select(v => new PetVaccinationDto
                {
                    VaccinationId = v.VaccinationId,
                    VaccineName = v.VaccineName,
                    DateAdministered = v.VaccinationDate,
                    NextDueDate = v.NextDueDate
                }).ToList()
            };

            dto.Prescriptions = pet.Appointments
                .SelectMany(a => a.Prescriptions)
                .OrderByDescending(p => p.StartDate)
                .Select(p => new PetPrescriptionDto
                {
                    PrescriptionId = p.PrescriptionId,
                    Date = p.CreatedDate,
                    MedicationName = p.Medication?.Name ?? "Unknown Drug",
                    Dosage = $"{p.Dosage} ({p.Frequency})",
                    AppointmentId = p.AppointmentId
                }).ToList();

            return dto;
        }
    }
}
