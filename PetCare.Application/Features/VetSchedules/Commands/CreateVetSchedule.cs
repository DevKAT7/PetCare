using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class CreateVetScheduleCommand : IRequest<int>
    {
        public required VetScheduleCreateModel Schedule { get; set; }
    }

    public class CreateVetScheduleHandler : IRequestHandler<CreateVetScheduleCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateVetScheduleHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateVetScheduleCommand request, CancellationToken cancellationToken)
        {
            var model = request.Schedule;

            var vetExists = await _context.Vets.AnyAsync(v => v.VetId == model.VetId, cancellationToken);

            if (!vetExists)
            {
                throw new NotFoundException("Vet not found.");
            }

            var schedule = new VetSchedule
            {
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                VetId = model.VetId
            };

            _context.VetSchedules.Add(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return schedule.VetScheduleId;
        }
    }
}
