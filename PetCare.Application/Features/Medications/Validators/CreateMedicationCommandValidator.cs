using FluentValidation;
using PetCare.Application.Features.Medications.Commands;
using PetCare.Application.Features.Medications.Dtos;

namespace PetCare.Application.Features.Medications.Validators
{
    public class CreateMedicationCommandValidator : AbstractValidator<CreateMedicationCommand>
    {
        public CreateMedicationCommandValidator(IValidator<MedicationCreateModel> medicationModelValidator)
        {
            RuleFor(x => x.Medication).SetValidator(medicationModelValidator);
        }
    }
}
