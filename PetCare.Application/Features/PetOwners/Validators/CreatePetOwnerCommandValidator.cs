using FluentValidation;
using PetCare.Application.Features.PetOwners.Commands;
using PetCare.Application.Features.PetOwners.Dtos;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class CreatePetOwnerCommandValidator : AbstractValidator<CreatePetOwnerCommand>
    {
        public CreatePetOwnerCommandValidator(IValidator<PetOwnerCreateModel> petOwnerModelValidator)
        {
            RuleFor(x => x.PetOwner).SetValidator(petOwnerModelValidator);
        }
    }
}
