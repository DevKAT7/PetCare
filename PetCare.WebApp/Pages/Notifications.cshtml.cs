using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Notifications.Commands;

namespace PetCare.WebApp.Pages
{
    //jeœli nie ma tokena (rzadkie, ale mo¿liwe na stronach bez formularzy),
    //fetch mo¿e zwróciæ 400. Wtedy potrzebne jest w³aœnie [IgnoreAntiforgeryToken]
    [IgnoreAntiforgeryToken]
    public class NotificationsModel : PageModel
    {
        private readonly IMediator _mediator;

        public NotificationsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnGet()
        {
            //ta strona nie ma widoku, s³u¿y tylko do obs³ugi ¿¹dañ
        }

        //to jest "Handler", który zastêpuje API Controller
        public async Task<IActionResult> OnPostMarkReadAsync(int id)
        {
            await _mediator.Send(new MarkAsReadNotificationCommand(id));
            return new JsonResult(new { success = true });
        }
    }
}
