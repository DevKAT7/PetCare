using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Exceptions;
using PetCare.Application.Features.Appointments.Dtos;
using PetCare.Core.Models;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Commands
{
    public class AddProcedureToAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; set; }
        public AppointmentProcedureCreateModel Model { get; set; } = null!;
    }

    public class UpdateProcedureInAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; set; }
        public int ProcedureId { get; set; }
        public AppointmentProcedureCreateModel Model { get; set; } = null!;
    }

    public class RemoveProcedureFromAppointmentCommand : IRequest<int>
    {
        public int AppointmentId { get; set; }
        public int ProcedureId { get; set; }
    }

    public class ManageAppointmentProcedureHandler :
        IRequestHandler<AddProcedureToAppointmentCommand, int>,
        IRequestHandler<UpdateProcedureInAppointmentCommand, int>,
        IRequestHandler<RemoveProcedureFromAppointmentCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public ManageAppointmentProcedureHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddProcedureToAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .Include(a => a.AppointmentProcedures)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.AppointmentId, cancellationToken);

            if (appointment == null)
            {
                throw new NotFoundException($"Appointment with id {request.AppointmentId} not found.");
            }

            //FindAsync jest uzywane dla kluczy i jest szybsze niz FirstOrDefaultAsync, najpierw sprawdza lokalna
            //pamiec kontekstu wiec jesli encja byla juz zaladowana to nie robi zapytania do bazy
            var procedure = await _context.Procedures.FindAsync(new object[] { request.Model.ProcedureId }, cancellationToken);

            if (procedure == null)
            {
                throw new NotFoundException($"Procedure with id {request.Model.ProcedureId} not found.");
            }

            var existing = appointment.AppointmentProcedures.FirstOrDefault(ap => ap.ProcedureId == request.Model.ProcedureId);
            var finalPrice = request.Model.FinalPrice ?? procedure.Price;

            if (existing != null)
            {
                existing.Quantity += request.Model.Quantity;
                existing.FinalPrice = finalPrice;
            }
            else
            {
                appointment.AppointmentProcedures.Add(new AppointmentProcedure
                {
                    AppointmentId = appointment.AppointmentId,
                    ProcedureId = procedure.ProcedureId,
                    Quantity = request.Model.Quantity,
                    FinalPrice = finalPrice
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return appointment.AppointmentId;
        }

        public async Task<int> Handle(UpdateProcedureInAppointmentCommand request, CancellationToken cancellationToken)
        {
            var ap = await _context.AppointmentProcedures
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId && x.ProcedureId == request.ProcedureId, cancellationToken);

            if (ap == null)
            {
                throw new NotFoundException("AppointmentProcedure", new { request.AppointmentId, request.ProcedureId });
            }

            var procedure = await _context.Procedures.FindAsync(new object[] { request.ProcedureId }, cancellationToken);

            if (procedure == null)
            {
                throw new NotFoundException($"Procedure with id {request.ProcedureId} not found.");
            }

            ap.Quantity = request.Model.Quantity;
            ap.FinalPrice = request.Model.FinalPrice ?? procedure.Price;

            await _context.SaveChangesAsync(cancellationToken);
            return ap.AppointmentId;
        }

        public async Task<int> Handle(RemoveProcedureFromAppointmentCommand request, CancellationToken cancellationToken)
        {
            var ap = await _context.AppointmentProcedures
                .FirstOrDefaultAsync(x => x.AppointmentId == request.AppointmentId && x.ProcedureId == request.ProcedureId, cancellationToken);

            if (ap == null)
            {
                throw new NotFoundException("AppointmentProcedure", new { request.AppointmentId, request.ProcedureId });
            }

            _context.AppointmentProcedures.Remove(ap);
            await _context.SaveChangesAsync(cancellationToken);

            return request.AppointmentId;
        }
    }
}
