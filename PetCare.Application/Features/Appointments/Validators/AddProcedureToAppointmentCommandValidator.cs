using FluentValidation;
using PetCare.Application.Features.Appointments.Commands;
using PetCare.Application.Features.Appointments.Dtos;

namespace PetCare.Application.Features.Appointments.Validators
{
    public class AddProcedureToAppointmentCommandValidator : AbstractValidator<AddProcedureToAppointmentCommand>
    {
        public AddProcedureToAppointmentCommandValidator(IValidator<AppointmentProcedureCreateModel> appointmentProcedureModelValidator)
        {
            RuleFor(x => x.Model).SetValidator(appointmentProcedureModelValidator);
        }
    }
}
