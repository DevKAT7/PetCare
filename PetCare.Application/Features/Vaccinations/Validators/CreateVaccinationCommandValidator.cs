using FluentValidation;
using PetCare.Application.Features.Vaccinations.Commands;
using PetCare.Application.Features.Vaccinations.Dtos;

namespace PetCare.Application.Features.Vaccinations.Validators
{
    public class CreateVaccinationCommandValidator : AbstractValidator<CreateVaccinationCommand>
    {
        public CreateVaccinationCommandValidator(IValidator<VaccinationCreateModel> vaccinationModelValidator)
        {
            RuleFor(x => x.Vaccination).SetValidator(vaccinationModelValidator);
        }
    }
}
