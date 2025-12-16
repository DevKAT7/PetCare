using FluentValidation;
using PetCare.Application.Features.Appointments.Dtos;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AppointmentProcedureCreateModelValidator : AbstractValidator<AppointmentProcedureCreateModel>
    {
        public AppointmentProcedureCreateModelValidator()
        {
            RuleFor(x => x.ProcedureId)
                .GreaterThan(0).WithMessage("ProcedureId jest wymagane.");
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Iloœæ musi byæ wiêksza lub równa 1.");
            RuleFor(x => x.FinalPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.FinalPrice.HasValue).WithMessage("Cena ostateczna nie mo¿e byæ wartoœci¹ ujemn¹.");
        }
    }
}
