using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Features.Medications.Dtos;
using PetCare.Application.Exceptions;
using PetCare.Application.Interfaces;

namespace PetCare.Application.Features.Medications.Commands
{
    public class UpdateMedicationCommand : IRequest<int>
    {
        public int Id { get; }
        public MedicationCreateModel Medication { get; set; }

        public UpdateMedicationCommand(int id, MedicationCreateModel model)
        {
            Id = id;
            Medication = model;
        }
    }

    public class UpdateMedicationHandler : IRequestHandler<UpdateMedicationCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateMedicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateMedicationCommand request, CancellationToken cancellationToken)
        {
            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == request.Id, cancellationToken);

            if (medication == null)
            {
                throw new NotFoundException("Medication", request.Id);
            }

            var model = request.Medication;

            medication.Name = model.Name;
            medication.Description = model.Description;
            medication.Manufacturer = model.Manufacturer;
            medication.Price = model.Price;
            medication.IsActive = model.IsActive;

            await _context.SaveChangesAsync(cancellationToken);

            return medication.MedicationId;
        }
    }
}
