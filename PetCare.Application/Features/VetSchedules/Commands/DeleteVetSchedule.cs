using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class DeleteVetScheduleCommand : IRequest<int>
    {
        public int VetScheduleId { get; }

        public DeleteVetScheduleCommand(int id) => VetScheduleId = id;
    }

    public class DeleteVetScheduleHandler : IRequestHandler<DeleteVetScheduleCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteVetScheduleHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteVetScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _context.VetSchedules
                .FirstOrDefaultAsync(s => s.VetScheduleId == request.VetScheduleId, cancellationToken);

            if (schedule == null)
            {
                throw new NotFoundException("Vet schedule not found.");
            }

            _context.VetSchedules.Remove(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return schedule.VetScheduleId;
        }
    }
}
