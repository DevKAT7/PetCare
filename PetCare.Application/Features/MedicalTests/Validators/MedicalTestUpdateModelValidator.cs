using FluentValidation;
using PetCare.Application.Features.MedicalTests.Dtos;

namespace PetCare.Application.Features.MedicalTests.Validators
{
    public class MedicalTestUpdateModelValidator : AbstractValidator<MedicalTestUpdateModel>
    {
        public MedicalTestUpdateModelValidator()
        {
            RuleFor(x => x.TestName)
                .NotEmpty().WithMessage("Test name is required.")
                .MaximumLength(200).WithMessage("Test name can have a maximum of 200 characters.");

            RuleFor(x => x.Result)
                .NotEmpty().WithMessage("Result is required.")
                .MaximumLength(2000).WithMessage("Result can have a maximum of 2000 characters.");

            RuleFor(x => x.AttachmentUrl)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Incorrect attachment URL.");
        }
    }
}
