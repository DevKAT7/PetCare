using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using System.Security.Claims;

namespace PetCare.WebApp.ViewComponents
{
    public class NotificationBarViewComponent : ViewComponent
    {
        private readonly IApplicationDbContext _context;

        public NotificationBarViewComponent(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Content(string.Empty);
            }

            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && n.ReadAt == null)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(notifications);
        }
    }
}
