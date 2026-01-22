using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PetCare.Application.Features.Vets.Queries;
using PetCare.Core.Models;

namespace PetCare.WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public IndexModel(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? Pesel { get; set; }
        public string? LicenseNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName;
            PhoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var vetProfile = await _mediator.Send(new GetVetByUserIdQuery(user.Id));

            if (vetProfile != null)
            {
                Address = vetProfile.Address;
                FullName = $"{vetProfile.FirstName} {vetProfile.LastName}";
                Pesel = vetProfile.Pesel;
                LicenseNumber = vetProfile.LicenseNumber;
                ProfilePictureUrl = vetProfile.ProfilePictureUrl;
            }

            return Page();
        }
    }
}
