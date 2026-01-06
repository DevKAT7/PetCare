using FluentValidation;
using PetCare.Application.Features.Procedures.Dtos;

namespace PetCare.Application.Features.Procedures.Validators
{
    public class ProcedureCreateModelValidator : AbstractValidator<ProcedureCreateModel>
    {
        public ProcedureCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa nie mo¿e przekraczaæ 100 znaków.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Cena nie mo¿e byæ wartoœci¹ ujemn¹.");

            RuleFor(x => x.VetSpecializationId)
                .GreaterThan(0).WithMessage("VetSpecializationId jest wymagany.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis nie mo¿e przekroczyæ 1000 znaków.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
