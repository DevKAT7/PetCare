using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.Notifications.Queries;
using System.Security.Claims;

namespace PetCare.WebApp.ViewComponents
{
    public class NotificationBarViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public NotificationBarViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Content(string.Empty);
            }

            var notifications = await _mediator.Send(new GetRecentUnreadNotificationsQuery(userId));

            return View(notifications);
        }
    }
}
