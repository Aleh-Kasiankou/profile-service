using Hangfire;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Services.EventSyncService;
using Idt.Profiles.Shared.ConfigurationOptions;
using Idt.Profiles.Shared.Constants;
using Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Idt.Profiles.Api.Extensions.Startup;

public static class StartupExtensions
{
    public static void ConfigureRecurringJobs(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetService<IRecurringJobManager>();

        var eventSyncService = application.Services.GetService<IEventSyncService>();
        if (eventSyncService is null)
        {
            throw new EventSyncJobNotRegisteredException();
        }

        recurringJobManager.AddOrUpdate(HangfireRecurringJobIds.OutboxEventSyncJobId,
            () => eventSyncService.SyncEvents(), Cron.Minutely);
    }

    public static async Task SetUpDbIndices(this WebApplication application)
    {
        var mongoClient = application.Services.GetRequiredService<IMongoClient>();
        var mongoOptions = application.Services.GetRequiredService<IOptions<MongoDbConfigurationOptions>>();
        var profilesDb = mongoClient.GetDatabase(mongoOptions.Value.Database);
        var profilesCollection = profilesDb.GetCollection<Profile>(mongoOptions.Value.ProfilesCollection);
        var profileIndexKeys = Builders<Profile>.IndexKeys;
        var uniqueProfileUsernameIndex = new CreateIndexModel<Profile>(profileIndexKeys.Ascending(x => x.UserName),
            new CreateIndexOptions { Unique = true });
        await profilesCollection.Indexes.CreateOneAsync(uniqueProfileUsernameIndex);
    }
}