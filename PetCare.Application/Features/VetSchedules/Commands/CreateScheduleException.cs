using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class CreateScheduleExceptionCommand : IRequest<int>
    {
        public required ScheduleExceptionCreateModel Exception { get; set; }
    }

    public class CreateScheduleExceptionHandler : IRequestHandler<CreateScheduleExceptionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateScheduleExceptionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var model = request.Exception;

            var vetExists = await _context.Vets.AnyAsync(v => v.VetId == model.VetId, cancellationToken);

            if (!vetExists)
            {
                throw new NotFoundException("Vet not found.");
            }

            var entity = new ScheduleException
            {
                ExceptionDate = model.ExceptionDate,
                IsFullDayAbsence = model.IsFullDayAbsence,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Reason = model.Reason,
                VetId = model.VetId
            };

            _context.ScheduleExceptions.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.ScheduleExceptionId;
        }
    }
}
