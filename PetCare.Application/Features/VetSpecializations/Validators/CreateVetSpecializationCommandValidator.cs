using FluentValidation;
using PetCare.Application.Features.VetSpecializations.Commands;
using PetCare.Application.Features.VetSpecializations.Dtos;

namespace PetCare.Application.Features.VetSpecializations.Validators
{
    public class CreateVetSpecializationCommandValidator : AbstractValidator<CreateVetSpecializationCommand>
    {
        public CreateVetSpecializationCommandValidator(IValidator<VetSpecializationCreateModel> specializationModelValidator)
        {
            RuleFor(x => x.Specialization).SetValidator(specializationModelValidator);
        }
    }
}
