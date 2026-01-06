using FluentValidation;
using PetCare.Application.Features.Notifications.Dtos;

namespace PetCare.Application.Features.Notifications.Validators
{
    public class NotificationCreateModelValidator : AbstractValidator<NotificationCreateModel>
    {
        public NotificationCreateModelValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Wiadomość jest wymagana.")
                .MaximumLength(500).WithMessage("Wiadomość może mieć maksymalnie 500 znaków.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Id użytkownika jest wymagane.");
        }
    }
}
