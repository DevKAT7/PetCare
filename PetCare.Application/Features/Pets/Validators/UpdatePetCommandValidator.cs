using FluentValidation;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Application.Features.Pets.Dtos;

namespace PetCare.Application.Features.Pets.Validators
{
    public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
    {
        public UpdatePetCommandValidator(IValidator<PetUpdateModel> petModelValidator)
        {
            RuleFor(x => x.Pet).SetValidator(petModelValidator);
        }
    }
}
