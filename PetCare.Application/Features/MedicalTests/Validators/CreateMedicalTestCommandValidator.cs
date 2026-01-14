using FluentValidation;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dtos;

namespace PetCare.Application.Features.MedicalTests.Validators
{
    public class CreateMedicalTestCommandValidator : AbstractValidator<CreateMedicalTestCommand>
    {
        public CreateMedicalTestCommandValidator(IValidator<MedicalTestCreateModel> medicalTestModelValidator)
        {
            RuleFor(x => x.MedicalTest).SetValidator(medicalTestModelValidator);
        }
    }
}
