using FluentValidation;
using PetCare.Application.Features.Notifications.Dtos;

namespace PetCare.Application.Features.Notifications.Validators
{
    public class NotificationCreateModelValidator : AbstractValidator<NotificationCreateModel>
    {
        public NotificationCreateModelValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(500).WithMessage("The message can have a maximum of 500 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User is required");
        }
    }
}
