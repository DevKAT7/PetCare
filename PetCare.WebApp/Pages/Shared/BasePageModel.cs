using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.PageTexts.Queries;

namespace PetCare.WebApp.Pages.Shared
{
    public class BasePageModel : PageModel
    {
        protected readonly IMediator _mediator;

        public BasePageModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Dictionary<string, string> PageTexts { get; set; } = new();

        protected async Task LoadPageTextsAsync()
        {
            PageTexts = await _mediator.Send(new GetAllPageTextsQuery());
            ViewData["PageTexts"] = PageTexts;
        }
    }
}
