using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Vaccinations.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vaccinations.Commands
{
    public class UpdateVaccinationCommand : IRequest<int>
    {
        public int Id { get; }
        public VaccinationUpdateModel Vaccination { get; set; }

        public UpdateVaccinationCommand(int id, VaccinationUpdateModel model)
        {
            Id = id;
            Vaccination = model;
        }
    }

    public class UpdateVaccinationHandler : IRequestHandler<UpdateVaccinationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVaccinationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateVaccinationCommand request, CancellationToken cancellationToken)
        {
            var vaccination = await _context.Vaccinations
                .FirstOrDefaultAsync(v => v.VaccinationId == request.Id, cancellationToken);

            if (vaccination == null)
            {
                throw new NotFoundException("Vaccination", request.Id);
            }

            var model = request.Vaccination;

            vaccination.VaccineName = model.VaccineName;
            vaccination.NextDueDate = model.NextDueDate;

            await _context.SaveChangesAsync(cancellationToken);

            return vaccination.VaccinationId;
        }
    }
}
