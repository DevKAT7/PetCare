using MediatR;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Procedures.Commands
{
    public class CreateProcedureCommand : IRequest<int>
    {
        public required ProcedureCreateModel Procedure { get; set; }
    }

    public class CreateProcedureHandler : IRequestHandler<CreateProcedureCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateProcedureHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProcedureCommand request, CancellationToken cancellationToken)
        {
            var model = request.Procedure;

            var procedure = new Procedure
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                VetSpecializationId = model.VetSpecializationId,
                IsActive = model.IsActive
            };

            _context.Procedures.Add(procedure);

            await _context.SaveChangesAsync(cancellationToken);

            return procedure.ProcedureId;
        }
    }
}