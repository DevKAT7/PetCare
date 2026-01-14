using FluentValidation;
using PetCare.Application.Features.Prescriptions.Commands;
using PetCare.Application.Features.Prescriptions.Dtos;

namespace PetCare.Application.Features.Prescriptions.Validators
{
    public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionCommandValidator(IValidator<PrescriptionCreateModel> prescriptionModelValidator)
        {
            RuleFor(x => x.Prescription).SetValidator(prescriptionModelValidator);
        }
    }
}
