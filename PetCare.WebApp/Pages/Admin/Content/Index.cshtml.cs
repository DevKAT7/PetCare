using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.PageTexts.Commands;
using PetCare.Application.Features.PageTexts.Queries;
using PetCare.Core.Models;
using ValidationException = PetCare.Application.Exceptions.ValidationException;

namespace PetCare.WebApp.Pages.Admin.Content
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator) => _mediator = mediator;

        public List<PageText> PageTexts { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public int SelectedId { get; set; }

        public class InputModel
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public async Task OnGetAsync()
        {
            PageTexts = await _mediator.Send(new GetAllPageTextsForAdminQuery());
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                PageTexts = await _mediator.Send(new GetAllPageTextsForAdminQuery());
                ViewData["ShowCreateModal"] = true;
                return Page();
            }

            try
            {
                await _mediator.Send(new CreatePageTextCommand
                {
                    Key = Input.Key,
                    Value = Input.Value
                });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var errorMessage in errorMessages)
                    {
                        ModelState.AddModelError($"Input.{propertyName}", errorMessage);
                    }
                }

                PageTexts = await _mediator.Send(new GetAllPageTextsForAdminQuery());
                ViewData["ShowCreateModal"] = true;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                PageTexts = await _mediator.Send(new GetAllPageTextsForAdminQuery());
                ViewData["ShowEditModal"] = true;
                return Page();
            }

            try
            {
                await _mediator.Send(new UpdatePageTextCommand
                {
                    Id = SelectedId,
                    Key = Input.Key,
                    Value = Input.Value
                });
            }
            catch (ValidationException ex)
            {
                foreach (var entry in ex.Errors)
                {
                    string propertyName = entry.Key;
                    string[] errorMessages = entry.Value;

                    foreach (var errorMessage in errorMessages)
                    {
                        ModelState.AddModelError($"Input.{propertyName}", errorMessage);
                    }
                }

                PageTexts = await _mediator.Send(new GetAllPageTextsForAdminQuery());
                ViewData["ShowEditModal"] = true;
                return Page();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            await _mediator.Send(new DeletePageTextCommand(SelectedId));
            return RedirectToPage();
        }
    }
}
