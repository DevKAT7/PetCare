using FluentValidation;
using PetCare.Application.Features.Procedures.Dtos;

namespace PetCare.Application.Features.Procedures.Validators
{
    public class ProcedureCreateModelValidator : AbstractValidator<ProcedureCreateModel>
    {
        public ProcedureCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be a positive value.");

            RuleFor(x => x.VetSpecializationId)
                .GreaterThan(0).WithMessage("Specialization is required.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
