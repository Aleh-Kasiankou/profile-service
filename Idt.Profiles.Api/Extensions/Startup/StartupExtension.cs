using Hangfire;
using Idt.Profiles.Services.EventSyncHostedService;
using Idt.Profiles.Shared.Exceptions;

namespace Idt.Profiles.Api.Extensions.Startup;

public static class StartupExtensions
{
    private const string SyncJobId = "SyncJobId";

    public static void ConfigureRecurringJobs(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetService<IRecurringJobManager>();

        var eventSyncService = application.Services.GetService<IEventSyncService>();
        if (eventSyncService is null)
        {
            throw new EventSyncJobNotRegisteredException();
        }

        recurringJobManager.AddOrUpdate(SyncJobId, () => eventSyncService.SyncEvents(), Cron.Minutely);
    }
}