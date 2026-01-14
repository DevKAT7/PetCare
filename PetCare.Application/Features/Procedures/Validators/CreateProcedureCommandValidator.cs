using FluentValidation;
using PetCare.Application.Features.Procedures.Commands;
using PetCare.Application.Features.Procedures.Dtos;

namespace PetCare.Application.Features.Procedures.Validators
{
    public class CreateProcedureCommandValidator : AbstractValidator<CreateProcedureCommand>
    {
        public CreateProcedureCommandValidator(IValidator<ProcedureCreateModel> procedureModelValidator)
        {
            RuleFor(x => x.Procedure).SetValidator(procedureModelValidator);
        }
    }
}
