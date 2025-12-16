using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Procedures.Dtos;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Procedures.Commands
{
    public class UpdateProcedureCommand : IRequest<int>
    {
        public UpdateProcedureCommand(int id, ProcedureCreateModel model)
        {
            ProcedureId = id;
            Model = model;
        }

        public int ProcedureId { get; }
        public ProcedureCreateModel Model { get; }
    }

    public class UpdateProcedureHandler : IRequestHandler<UpdateProcedureCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public UpdateProcedureHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateProcedureCommand request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var procedure = await _context.Procedures.FirstOrDefaultAsync(p => p.ProcedureId == request.ProcedureId, cancellationToken);

            if (procedure == null)
            {
                throw new NotFoundException($"Procedure with id {request.ProcedureId} not found.");
            }

            procedure.Name = model.Name;
            procedure.Description = model.Description;
            procedure.Price = model.Price;
            procedure.VetSpecializationId = model.VetSpecializationId;
            procedure.IsActive = model.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
            return procedure.ProcedureId;
        }
    }
}