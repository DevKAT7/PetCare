using FluentValidation;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dto;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator(IValidator<AppointmentCreateModel> appointmentModelValidator)
        {
            RuleFor(x => x.Appointment).SetValidator(appointmentModelValidator);
        }
    }
}
