using FluentValidation;
using PetCare.Application.Features.MedicalTests.Commands;
using PetCare.Application.Features.MedicalTests.Dtos;

namespace PetCare.Application.Features.MedicalTests.Validators
{
    public class UpdateMedicalTestCommandValidator : AbstractValidator<UpdateMedicalTestCommand>
    {
        public UpdateMedicalTestCommandValidator(IValidator<MedicalTestUpdateModel> medicalTestModelValidator)
        {
            RuleFor(x => x.MedicalTest).SetValidator(medicalTestModelValidator);
        }
    }
}
