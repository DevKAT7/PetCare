using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetCare.Application.Features.Appointments.Dto;
using PetCare.Application.Features.Appointments.Queries;
using PetCare.Application.Features.Vets.Queries;

namespace PetCare.WebApp.Pages
{
    [Authorize(Roles = "Admin, Employee")]
    public class CalendarModel : PageModel
    {
        private readonly IMediator _mediator;

        public CalendarModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<AppointmentReadModel> Appointments { get; set; } = new();
        public List<DateTime> WeekDays { get; set; } = new();
        public List<TimeSpan> TimeSlots { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? SelectedVetId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? CurrentDate { get; set; }

        public SelectList? VetOptions { get; set; }

        public async Task OnGetAsync()
        {
            //Calculate Date Range (Monday to Friday)
            var anchorDate = CurrentDate ?? DateTime.Today;
            //Calculate offset to get to Monday
            int diff = (7 + (anchorDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = anchorDate.AddDays(-1 * diff).Date;

            WeekDays = Enumerable.Range(0, 5)
                .Select(i => monday.AddDays(i))
                .ToList();

            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(16, 30, 0);
            TimeSlots = new List<TimeSpan>();
            while (startTime <= endTime)
            {
                TimeSlots.Add(startTime);
                startTime = startTime.Add(TimeSpan.FromMinutes(30));
            }

            var vetQuery = new GetAllVetsQuery();
            var vets = await _mediator.Send(vetQuery);
            VetOptions = new SelectList(vets, "VetId", "FullName");

            //fetch from Monday 00:00 to Friday 23:59
            var appQuery = new GetAllAppointmentsQuery(
                vetId: SelectedVetId,
                from: WeekDays.First(),
                to: WeekDays.Last().AddDays(1).AddTicks(-1)
            );

            Appointments = await _mediator.Send(appQuery);
        }
    }
}