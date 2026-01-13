using FluentValidation;
using PetCare.Application.Features.Vaccinations.Dtos;

namespace PetCare.Application.Features.Vaccinations.Validators
{
    public class VaccinationCreateModelValidator : AbstractValidator<VaccinationCreateModel>
    {
        public VaccinationCreateModelValidator()
        {
            RuleFor(x => x.VaccineName)
                .NotEmpty().WithMessage("Vaccine name is required.")
                .MaximumLength(200).WithMessage("Vaccine name can have a maximum of 200 characters.");

            RuleFor(x => x.VaccinationDate)
                .NotEmpty().WithMessage("Vaccination date is required.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("Pet is required.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment is required.");
        }
    }
}
