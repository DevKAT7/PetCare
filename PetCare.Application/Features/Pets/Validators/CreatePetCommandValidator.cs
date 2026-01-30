using FluentValidation;
using PetCare.Application.Features.Pets.Commands;
using PetCare.Shared.Dtos;

namespace PetCare.Application.Features.Pets.Validators
{
    public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
    {
        public CreatePetCommandValidator(IValidator<PetCreateModel> petModelValidator)
        {
            RuleFor(x => x.Pet).SetValidator(petModelValidator);
        }
    }
}
