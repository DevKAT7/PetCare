using FluentValidation;
using PetCare.Application.Features.Vets.Commands;
using PetCare.Application.Features.Vets.Dto;

namespace PetCare.Application.Features.Vets.Validators
{
    public class UpdateVetCommandValidator : AbstractValidator<UpdateVetCommand>
    {
        public UpdateVetCommandValidator(IValidator<VetUpdateModel> vetModelValidator)
        {
            RuleFor(x => x.Vet).SetValidator(vetModelValidator);
        }
    }
}
