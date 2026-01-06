using FluentValidation;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dtos;

namespace PetCare.Application.Features.Vets.Validators
{
    public class CreateVetCommandValidator : AbstractValidator<CreateVetCommand>
    {
        public CreateVetCommandValidator(IValidator<VetCreateModel> vetModelValidator)
        {
            RuleFor(x => x.Vet).SetValidator(vetModelValidator);
        }
    }
}
