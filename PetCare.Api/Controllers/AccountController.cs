using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Features.PetOwners.Commands;
using PetCare.Application.Features.PetOwners.Dtos;
using PetCare.Application.Features.PetOwners.Queries;
using PetCare.Core.Models;
using PetCare.Shared.Dtos;

namespace PetCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public AccountController(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");

            var petOwner = await _mediator.Send(new GetPetOwnerByUserIdQuery(user.Id));

            if (petOwner == null) return NotFound("Pet Owner profile not found");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserProfileDto
            {
                Email = user.Email!,
                FirstName = petOwner.FirstName,
                LastName = petOwner.LastName,
                PhoneNumber = user.PhoneNumber ?? "",
                Address = petOwner.Address,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new { ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)) });
            }

            return Ok(new { Success = true });
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] EditProfileDto request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("User not found.");

            var petOwnerDto = await _mediator.Send(new GetPetOwnerByUserIdQuery(user.Id));

            if (petOwnerDto == null)
            {
                return NotFound("Profil właściciela zwierzęcia nie został znaleziony.");
            }

            var updateModel = new PetOwnerUpdateModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };

            var command = new UpdatePetOwnerCommand(petOwnerDto.PetOwnerId, updateModel);

            await _mediator.Send(command);

            return Ok(new { Success = true });
        }
    }
}
