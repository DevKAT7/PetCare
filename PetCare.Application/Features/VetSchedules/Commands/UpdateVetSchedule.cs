using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSchedules.Dtos;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class UpdateVetScheduleCommand : IRequest<int>
    {
        public int Id { get; }
        public VetScheduleCreateModel Schedule { get; set; }

        public UpdateVetScheduleCommand(int id, VetScheduleCreateModel schedule)
        {
            Id = id;
            Schedule = schedule;
        }
    }

    public class UpdateVetScheduleHandler : IRequestHandler<UpdateVetScheduleCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateVetScheduleHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateVetScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _context.VetSchedules.FirstOrDefaultAsync(s => s.VetScheduleId == request.Id, cancellationToken);

            if (schedule == null)
            {
                throw new NotFoundException("Vet schedule", request.Id);
            }

            if (schedule.VetId != request.Schedule.VetId)
            {
                var vetExists = await _context.Vets.AnyAsync(v => v.VetId == request.Schedule.VetId, cancellationToken);

                if (!vetExists)
                {
                    throw new NotFoundException("Vet not found.");
                }
            }

            var model = request.Schedule;

            schedule.DayOfWeek = model.DayOfWeek;
            schedule.StartTime = model.StartTime;
            schedule.EndTime = model.EndTime;
            schedule.VetId = model.VetId;

            await _context.SaveChangesAsync(cancellationToken);

            return schedule.VetScheduleId;
        }
    }
}
