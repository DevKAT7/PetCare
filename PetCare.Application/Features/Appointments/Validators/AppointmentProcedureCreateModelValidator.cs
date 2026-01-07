using FluentValidation;
using PetCare.Application.Features.Appointments.Dtos;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AppointmentProcedureCreateModelValidator : AbstractValidator<AppointmentProcedureCreateModel>
    {
        public AppointmentProcedureCreateModelValidator()
        {
            RuleFor(x => x.ProcedureId)
                .NotNull().WithMessage("Procedure is required.")
                .GreaterThan(0).WithMessage("Procedure is required.");
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Quantity must be greater than or equal to 1.");
            RuleFor(x => x.FinalPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.FinalPrice.HasValue).WithMessage("Final price cannot be a negative value.");
        }
    }
}
