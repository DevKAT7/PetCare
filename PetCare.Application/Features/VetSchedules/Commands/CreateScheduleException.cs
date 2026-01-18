using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Features.VetSchedules.Dto;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;
using PetCare.Core.Models;

namespace PetCare.Application.Features.VetSchedules.Commands
{
    public class CreateScheduleExceptionCommand : IRequest<int>
    {
        public required ScheduleExceptionCreateModel Exception { get; set; }
    }

    public class CreateScheduleExceptionHandler : IRequestHandler<CreateScheduleExceptionCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CreateScheduleExceptionHandler(IApplicationDbContext context, UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateScheduleExceptionCommand request, CancellationToken cancellationToken)
        {
            var model = request.Exception;

            var vet = await _context.Vets.Include(v => v.User).FirstOrDefaultAsync(v => v.VetId == model.VetId, cancellationToken);

            if (vet == null)
            {
                throw new NotFoundException("Vet not found.");
            }

            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            var entity = new ScheduleException
            {
                ExceptionDate = model.ExceptionDate,
                IsFullDayAbsence = model.IsFullDayAbsence,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Reason = model.Reason,
                VetId = model.VetId,
                Status = isAdmin ? ScheduleExceptionStatus.Approved : ScheduleExceptionStatus.Pending
            };

            _context.ScheduleExceptions.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            if (entity.Status == ScheduleExceptionStatus.Pending)
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                foreach (var admin in admins)
                {
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        Notification = new NotificationCreateModel
                        {
                            UserId = admin.Id,
                            Type = NotificationType.VacationRequest,
                            Message = $"New leave request from {vet.FirstName} {vet.LastName} on {entity.ExceptionDate:yyyy-MM-dd}."
                        }
                    });
                }
            }

            return entity.ScheduleExceptionId;
        }
    }
}
