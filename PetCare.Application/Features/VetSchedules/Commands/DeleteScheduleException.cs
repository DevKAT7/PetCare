using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class DeleteScheduleExceptionCommand : IRequest<int>
    {
        public int Id { get; }
        public DeleteScheduleExceptionCommand(int id) => Id = id;
    }

    public class DeleteScheduleExceptionHandler : IRequestHandler<DeleteScheduleExceptionCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteScheduleExceptionHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var exception = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(e => e.ScheduleExceptionId == request.Id, cancellationToken);

            if (exception == null)
            {
                throw new NotFoundException("ScheduleException", request.Id);
            }

            _context.ScheduleExceptions.Remove(exception);

            await _context.SaveChangesAsync(cancellationToken);

            return exception.ScheduleExceptionId;
        }
    }
}
