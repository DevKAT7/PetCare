using FluentValidation;
using PetCare.Application.Features.VetSpecializations.Dtos;

namespace PetCare.Application.Features.VetSpecializations.Validators
{
    public class VetSpecializationCreateModelValidator : AbstractValidator<VetSpecializationCreateModel>
    {
        public VetSpecializationCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .MaximumLength(50).WithMessage("Nazwa nie mo¿e przekraczaæ 50 znaków.");
        }
    }
}
