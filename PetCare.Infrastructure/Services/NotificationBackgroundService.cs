using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceProvider serviceProvider, 
            ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var checkInterval = TimeSpan.FromHours(1);

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
                            var freshAppt = await context.Appointments
                                .FirstOrDefaultAsync(a => a.AppointmentId == appt.AppointmentId && !a.IsReminderSent, stoppingToken);

                            if (freshAppt == null) continue;

                            freshAppt.IsReminderSent = true;

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
                        }

                        var tomorrowInvoice = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

                        var upcomingInvoices = await context.Invoices
                            .Include(i => i.PetOwner)
                            .Where(i => !i.IsPaid
                                     && i.DueDate == tomorrowInvoice
                                     && !i.IsDueReminderSent)
                            .ToListAsync(stoppingToken);

                        foreach (var inv in upcomingInvoices)
                        {
                            if (inv.PetOwner?.UserId != null)
                            {
                                await mediator.Send(new CreateNotificationCommand
                                {
                                    Notification = new NotificationCreateModel
                                    {
                                        UserId = inv.PetOwner.UserId,
                                        Type = NotificationType.InvoiceDue,
                                        Message = $"Reminder: Payment for invoice #{inv.InvoiceNumber} is due tomorrow."
                                    }
                                });
                            }

                            inv.IsDueReminderSent = true;
                        }

                        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

                        var overdueInvoices = await context.Invoices
                            .Include(i => i.PetOwner)
                            .Where(i => !i.IsPaid
                                     && i.DueDate == yesterday
                                     && !i.IsOverdueReminderSent)
                            .ToListAsync(stoppingToken);

                        foreach (var inv in overdueInvoices)
                        {
                            if (inv.PetOwner?.UserId != null)
                            {
                                await mediator.Send(new CreateNotificationCommand
                                {
                                    Notification = new NotificationCreateModel
                                    {
                                        UserId = inv.PetOwner.UserId,
                                        Type = NotificationType.InvoiceOverdue,
                                        Message = $"Overdue: Payment for invoice #{inv.InvoiceNumber} was due yesterday."
                                    }
                                });
                            }
                            inv.IsOverdueReminderSent = true;
                        }

                        var warningDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3));

                        var vaccinationsDue = await context.Vaccinations
                            .Include(v => v.Pet).ThenInclude(p => p.PetOwner)
                            .Where(v => v.NextDueDate.HasValue
                                     && v.NextDueDate.Value == warningDate
                                     && !v.IsReminderSent)
                            .ToListAsync(stoppingToken);

                        foreach (var vacc in vaccinationsDue)
                        {
                            if (vacc.Pet?.PetOwner?.UserId != null)
                            {
                                await mediator.Send(new CreateNotificationCommand
                                {
                                    Notification = new NotificationCreateModel
                                    {
                                        UserId = vacc.Pet.PetOwner.UserId,
                                        Type = NotificationType.VaccineDue,
                                        Message = $"Vaccination Reminder: {vacc.VaccineName} for {vacc.Pet.Name} is due on {vacc.NextDueDate:yyyy-MM-dd}."
                                    }
                                });
                            }

                            vacc.IsReminderSent = true;
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing the NotificationBackgroundService.");
                }

                await Task.Delay(checkInterval, stoppingToken);
            }
        }
    }
}
