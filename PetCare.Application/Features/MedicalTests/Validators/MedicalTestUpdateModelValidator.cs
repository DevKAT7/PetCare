using FluentValidation;
using PetCare.Application.Features.MedicalTests.Dtos;
using System;

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
                .Must(uri => string.IsNullOrEmpty(uri)
                            || uri.StartsWith("/")
                            || uri.StartsWith("http")
                            || (!uri.Contains("/") && !uri.Contains("\\") && uri.Contains("."))
                            || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Incorrect attachment URL. Must be a valid link or a filename.");
        }
    }
}
