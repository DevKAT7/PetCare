using FluentValidation;
using PetCare.Application.Features.Vets.Commands;

namespace PetCare.Application.Features.Vets.Validators
{
    public class CreateVetCommandValidator : AbstractValidator<CreateVetCommand>
    {
        public CreateVetCommandValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                    .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage("Phone number is required.")
              .MinimumLength(9).WithMessage("Phone number must be at least 9 characters long.")
              .Matches("^[0-9+() -]*$").WithMessage("Invalid phone number format.")
              .MaximumLength(20).WithMessage("Phone number cannot be longer than 20 characters.");

            RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage("First name is required.")
              .MaximumLength(100).WithMessage("First name cannot be longer than 100 characters.");

            RuleFor(x => x.LastName)
              .NotEmpty().WithMessage("Last name is required.")
              .MaximumLength(100).WithMessage("Last name cannot be longer than 100 characters.");

            RuleFor(x => x.Pesel)
              .NotEmpty().WithMessage("PESEL is required.")
              .Length(11).WithMessage("PESEL must have exactly 11 digits.")
              .Matches("^[0-9]*$").WithMessage("PESEL must consist of digits only.");

            RuleFor(x => x.LicenseNumber)
              .NotEmpty().WithMessage("License number is required.")
              .MaximumLength(50).WithMessage("License number is too long.");

            RuleFor(x => x.HireDate)
                .NotEmpty().WithMessage("Hire date is required.")
                .Must(date => date.Date <= DateTime.Today)
                .WithMessage("Hire date cannot be in the future.");

            RuleFor(x => x.CareerStartDate)
              .NotEmpty().WithMessage("Career start date is required.")
              .Must(date => date.Date <= DateTime.Today)
              .WithMessage("Career start date cannot be in the future.");

            RuleFor(x => x.Address)
              .NotEmpty().WithMessage("Address is required.")
              .MaximumLength(500).WithMessage("Address cannot be longer than 500 characters.");

            RuleFor(x => x.ProfilePictureUrl)
              .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
              .WithMessage("Invalid URL format.");

            RuleFor(x => x.Description)
              .NotEmpty().WithMessage("Description is required.")
              .MaximumLength(2000).WithMessage("Description cannot be longer than 2000 characters.");

            RuleFor(x => x.SpecializationIds)
                .NotEmpty().WithMessage("At least one specialization is required.");

            RuleForEach(x => x.SpecializationIds).GreaterThan(0).WithMessage("Invalid specialization ID.");

            RuleFor(x => x).Custom((m, ctx) =>
            {
                if (m.CareerStartDate > m.HireDate)
                {
                    ctx.AddFailure("CareerStartDate", "Career start date cannot be later than hire date.");
                }
            });
        }
    }
}