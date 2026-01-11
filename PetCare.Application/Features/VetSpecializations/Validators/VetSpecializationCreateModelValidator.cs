using FluentValidation;
using PetCare.Application.Features.VetSpecializations.Dtos;

namespace PetCare.Application.Features.VetSpecializations.Validators
{
    public class VetSpecializationCreateModelValidator : AbstractValidator<VetSpecializationCreateModel>
    {
        public VetSpecializationCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
        }
    }
}
