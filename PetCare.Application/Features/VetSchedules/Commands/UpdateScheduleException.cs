using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class UpdateScheduleExceptionCommand : IRequest<int>
    {
        public int Id { get; }
        public ScheduleExceptionCreateModel Exception { get; set; }

        public UpdateScheduleExceptionCommand(int id, ScheduleExceptionCreateModel exception)
        {
            Id = id;
            Exception = exception;
        }
    }

    public class UpdateScheduleExceptionHandler : IRequestHandler<UpdateScheduleExceptionCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateScheduleExceptionHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(UpdateScheduleExceptionCommand request, CancellationToken cancellationToken)
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
                throw new ValidationException("Status", "Only Admins can modify processed requests.");
            }

            var model = request.Exception;

            exception.ExceptionDate = model.ExceptionDate;
            exception.IsFullDayAbsence = model.IsFullDayAbsence;
            exception.StartTime = model.StartTime;
            exception.EndTime = model.EndTime;
            exception.Reason = model.Reason;
            exception.VetId = model.VetId;

            await _context.SaveChangesAsync(cancellationToken);

            return exception.ScheduleExceptionId;
        }
    }
}
