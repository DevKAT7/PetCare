using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteScheduleExceptionHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(DeleteScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var exception = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(e => e.ScheduleExceptionId == request.Id, cancellationToken);

            if (exception == null)
            {
                throw new NotFoundException("ScheduleException", request.Id);
            }

            var user = _httpContextAccessor.HttpContext?.User;
            bool isAdmin = user != null && user.IsInRole("Admin");

            if (exception.Status != ScheduleExceptionStatus.Pending && !isAdmin)
            {
                throw new ValidationException("Status", "Only Admins can delete processed requests.");
            }

            _context.ScheduleExceptions.Remove(exception);

            await _context.SaveChangesAsync(cancellationToken);

            return exception.ScheduleExceptionId;
        }
    }
}
