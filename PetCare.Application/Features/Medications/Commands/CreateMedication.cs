using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Application.Exceptions;
using PetCare.Core.Models;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Medications.Commands
{
    public class CreateMedicationCommand : IRequest<int>
    {
        public required MedicationCreateModel Medication { get; set; }
    }

    public class CreateMedicationHandler : IRequestHandler<CreateMedicationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateMedicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
        {
            var model = request.Medication;

            var exists = await _context.Medications.AnyAsync(m => m.Name == model.Name, cancellationToken);
            if (exists)
            {
                throw new BadRequestException($"Medication '{model.Name}' already exists.");
            }

            var entity = new Medication
            {
                Name = model.Name,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
                Price = model.Price,
                IsActive = model.IsActive,

                StockItem = new StockItem
                {
                   CurrentStock = 0,
                   ReorderLevel = 10
                }
            };

            _context.Medications.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.MedicationId;
        }
    }
}
