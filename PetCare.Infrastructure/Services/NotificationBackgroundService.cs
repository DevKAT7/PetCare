using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetCare.Application.Features.Notifications.Commands;
using PetCare.Application.Features.Notifications.Dtos;
using PetCare.Application.Interfaces;
using PetCare.Core.Enums;

namespace PetCare.Infrastructure.Services
{
    //to jest dla powiadomień które nie wywołują się bezpośrednio z akcji użytkownika
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                        var tomorrow = DateTime.Today.AddDays(1);
                        var appointments = await context.Appointments
                            .Include(a => a.Vet)
                            .Include(a => a.Pet).ThenInclude(p => p.PetOwner)
                            .Where(a => a.AppointmentDateTime.Date == tomorrow && !a.IsReminderSent)
                            .ToListAsync(stoppingToken);

                        foreach (var appt in appointments)
                        {
                            // powiadomienie dla właściciela
                            await mediator.Send(new CreateNotificationCommand
                            {
                                Notification = new NotificationCreateModel
                                {
                                    UserId = appt.Pet.PetOwner.UserId,
                                    Type = NotificationType.AppointmentReminder,
                                    Message = $"Reminder: You have an appointment tomorrow at {appt.AppointmentDateTime:HH:mm} with {appt.Pet.Name}."
                                }
                            });

                            // powiadomienie dla lekarza
                            await mediator.Send(new CreateNotificationCommand
                            {
                                Notification = new NotificationCreateModel
                                {
                                    UserId = appt.Vet.UserId,
                                    Type = NotificationType.AppointmentReminder,
                                    Message = $"Reminder: Appointment tomorrow at {appt.AppointmentDateTime:HH:mm} (Patient: {appt.Pet.Name})."
                                }
                            });

                            appt.IsReminderSent = true;
                        }

                        if (appointments.Any())
                        {
                            await context.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: Add Logger
                    Console.WriteLine($"Background Service Error: {ex.Message}");
                }

                //sprawdzaj co godzinę
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
