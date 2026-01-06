using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCare.Infrastructure.Data;

namespace PetCare.Application.Features.Appointments.Queries
{
    public class GetVetAvailabilityQuery : IRequest<List<TimeSpan>>
    {
        public int VetId { get; set; }
        public DateTime Date { get; set; }
        public int AppointmentDurationMinutes { get; set; } = 30;
    }

    public class GetVetAvailabilityHandler : IRequestHandler<GetVetAvailabilityQuery, List<TimeSpan>>
    {
        private readonly ApplicationDbContext _context;

        public GetVetAvailabilityHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TimeSpan>> Handle(GetVetAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var requestedDate = DateOnly.FromDateTime(request.Date);
            var dayOfWeek = request.Date.DayOfWeek;

            var schedule = await _context.VetSchedules
                .FirstOrDefaultAsync(s => s.VetId == request.VetId && s.DayOfWeek == dayOfWeek, cancellationToken);

            if (schedule == null) return new List<TimeSpan>();

            TimeSpan workStartTime = schedule.StartTime.ToTimeSpan();
            TimeSpan workEndTime = schedule.EndTime.ToTimeSpan();

            var exception = await _context.ScheduleExceptions
                .FirstOrDefaultAsync(e => e.VetId == request.VetId && e.ExceptionDate == requestedDate, cancellationToken);

            if (exception != null && exception.IsFullDayAbsence)
            {
                return new List<TimeSpan>();
            }

            var appointments = await _context.Appointments
                .Where(a => a.VetId == request.VetId
                            && a.AppointmentDateTime.Date == request.Date.Date
                            && a.Status != Core.Enums.AppointmentStatus.Cancelled)
                .Select(a => new { a.AppointmentDateTime })
                .ToListAsync(cancellationToken);

            var availableSlots = new List<TimeSpan>();
            var slotTime = workStartTime;

            //Pętla po godzinach pracy (np. od 10:00 do 15:00)
            while (slotTime.Add(TimeSpan.FromMinutes(request.AppointmentDurationMinutes)) <= workEndTime)
            {
                //Definiujemy ramy czasowe tego konkretnego slotu (np. 10:00 - 10:30)
                var slotStart = slotTime;
                var slotEnd = slotTime.Add(TimeSpan.FromMinutes(request.AppointmentDurationMinutes));

                //Sprawdź kolizję z wizytami
                bool isTakenByAppointment = appointments.Any(a => a.AppointmentDateTime.TimeOfDay == slotStart);

                //Sprawdź kolizję z wyjątkiem
                bool isBlockedByException = false;
                if (exception != null && exception.StartTime.HasValue && exception.EndTime.HasValue)
                {
                    var exceptionStart = exception.StartTime.Value.ToTimeSpan();
                    var exceptionEnd = exception.EndTime.Value.ToTimeSpan();

                    // Sprawdzamy czy slot nakłada się na przerwę
                    // Slot nakłada się na przerwę, jeśli:
                    // (StartSlotu < KoniecPrzerwy) ORAZ (KoniecSlotu > StartPrzerwy)
                    if (slotStart < exceptionEnd && slotEnd > exceptionStart)
                    {
                        isBlockedByException = true;
                    }
                }

                // Dodajemy slot tylko jeśli jest wolny od wizyt I wolny od wyjątków
                if (!isTakenByAppointment && !isBlockedByException)
                {
                    // Sprawdzenie czy slot nie jest w przeszłości (jeśli "dzisiaj")
                    if (request.Date.Date > DateTime.Today || (request.Date.Date == DateTime.Today && slotStart > DateTime.Now.TimeOfDay))
                    {
                        availableSlots.Add(slotStart);
                    }
                }

                slotTime = slotTime.Add(TimeSpan.FromMinutes(request.AppointmentDurationMinutes));
            }

            return availableSlots;
        }
    }
}
