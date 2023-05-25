using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Shared.ConfigurationOptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using MQTTnet;
using MQTTnet.Client;

namespace Idt.Profiles.Services.EventSyncHostedService.Implementations;

public class EventSyncService : IEventSyncService
{
    private static readonly FilterDefinition<OutboxEvent> NotSentFilter =
        Builders<OutboxEvent>.Filter.Eq(x => x.Sent, false);

    private readonly IMongoCollection<OutboxEvent> _outboxCollection;
    private readonly IMqttClient _messageBrokerService;
    private readonly IList<Guid> _syncedEventIds = new List<Guid>();

    public EventSyncService(IOptions<MongoDbConfigurationOptions> mongoDbConfiguration,
        IMqttClient messageBrokerService)
    {
        _messageBrokerService = messageBrokerService;
        var mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoDbConfiguration.Value.Database);
        _outboxCollection =
            database.GetCollection<OutboxEvent>(mongoDbConfiguration.Value.EventOutboxCollection);
    }

    public async Task SyncEvents()
    {
        var eventsToSync = await (await _outboxCollection.FindAsync(NotSentFilter))
            .ToListAsync();
        try
        {
            await _messageBrokerService.ConnectAsync(new MqttClientOptions());
            foreach (var profileEvent in eventsToSync)
            {
                var messageToSend = new MqttApplicationMessageBuilder().WithTopic(profileEvent.Topic)
                    .WithPayload(profileEvent.Payload.ToByteArray()).Build();
                var publishResult = await _messageBrokerService.PublishAsync(messageToSend);
                if (publishResult.IsSuccess)
                {
                    _syncedEventIds.Add(profileEvent.OutboxEventId);
                }
            }
        }
        finally
        {
            await _messageBrokerService.DisconnectAsync();
            var eventSentToBrokerIdFilter = Builders<OutboxEvent>.Filter
                .Where(x => _syncedEventIds.Contains(x.OutboxEventId));
            var setSentToTrueUpdateDefinition =
                new UpdateDefinitionBuilder<OutboxEvent>().Set(x => x.Sent, true);
            await _outboxCollection.UpdateManyAsync(eventSentToBrokerIdFilter, setSentToTrueUpdateDefinition);
        }
    }
}