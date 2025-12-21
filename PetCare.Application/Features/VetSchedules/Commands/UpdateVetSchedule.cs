using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

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
        private readonly ApplicationDbContext _context;

        public UpdateVetScheduleHandler(ApplicationDbContext context)
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

            //TODO: czy chce miec tutaj veta do zmiany?
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
