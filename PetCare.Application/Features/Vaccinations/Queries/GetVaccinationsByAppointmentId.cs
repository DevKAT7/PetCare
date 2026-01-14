using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vaccinations.Queries
{
    public class GetVaccinationsByAppointmentIdQuery : IRequest<List<VaccinationReadModel>>
    {
        public int AppointmentId { get; set; }
    }

    public class GetVaccinationsByAppointmentIdHandler : IRequestHandler<GetVaccinationsByAppointmentIdQuery, List<VaccinationReadModel>>
    {
        private readonly IApplicationDbContext _context;

        public GetVaccinationsByAppointmentIdHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<VaccinationReadModel>> Handle(GetVaccinationsByAppointmentIdQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.Vaccinations
                .Where(v => v.AppointmentId == request.AppointmentId)
                .ToListAsync(cancellationToken);

            return items.Select(v => new VaccinationReadModel
            {
                VaccinationId = v.VaccinationId,
                VaccineName = v.VaccineName,
                VaccinationDate = v.VaccinationDate,
                NextDueDate = v.NextDueDate,
                PetId = v.PetId,
                AppointmentId = v.AppointmentId
            }).ToList();
        }
    }
}
