using FluentValidation;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dtos;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentCommandValidator(IValidator<AppointmentUpdateModel> appointmentModelValidator)
        {
            RuleFor(x => x.Appointment).SetValidator(appointmentModelValidator);
        }
    }
}
