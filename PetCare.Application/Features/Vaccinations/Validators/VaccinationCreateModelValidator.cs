using FluentValidation;
using PetCare.Application.Features.Vaccinations.Dtos;

namespace PetCare.Application.Features.Vaccinations.Validators
{
    public class VaccinationCreateModelValidator : AbstractValidator<VaccinationCreateModel>
    {
        public VaccinationCreateModelValidator()
        {
            RuleFor(x => x.VaccineName)
                .NotEmpty().WithMessage("Nazwa szczepionki jest wymagana.")
                .MaximumLength(200).WithMessage("Nazwa może mieć maksymalnie 200 znaków.");

            RuleFor(x => x.VaccinationDate)
                .NotEmpty().WithMessage("Data szczepienia jest wymagana.");

            RuleFor(x => x.PetId)
                .GreaterThan(0).WithMessage("PetId jest wymagane.");

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("AppointmentId jest wymagane.");
        }
    }
}
