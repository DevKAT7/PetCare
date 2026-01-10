using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Vaccinations.Commands
{
    public class DeleteVaccinationCommand : IRequest<int>
    {
        public int Id { get; }
        public DeleteVaccinationCommand(int id) => Id = id;
    }

    public class DeleteVaccinationHandler : IRequestHandler<DeleteVaccinationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteVaccinationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
        {
            var vaccination = await _context.Vaccinations
                .FirstOrDefaultAsync(v => v.VaccinationId == request.Id, cancellationToken);

            if (vaccination == null)
            {
                throw new NotFoundException("Vaccination", request.Id);
            }

            _context.Vaccinations.Remove(vaccination);

            await _context.SaveChangesAsync(cancellationToken);

            return vaccination.VaccinationId;
        }
    }
}
