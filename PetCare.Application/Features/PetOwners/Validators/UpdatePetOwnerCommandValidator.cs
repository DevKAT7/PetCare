using FluentValidation;
using PetCare.Application.Features.PetOwners.Commands;
using PetCare.Application.Features.PetOwners.Dtos;

namespace PetCare.Application.Features.PetOwners.Validators
{
    public class UpdatePetOwnerCommandValidator : AbstractValidator<UpdatePetOwnerCommand>
    {
        public UpdatePetOwnerCommandValidator(IValidator<PetOwnerUpdateModel> petOwnerModelValidator)
        {
            RuleFor(x => x.PetOwner).SetValidator(petOwnerModelValidator);
        }
    }
}
