
using MongoDB.Bson.Serialization.Attributes;
using MQTTnet;

namespace Idt.Profiles.Persistence.Models;

public class OutboxEvent
{
    public OutboxEvent(string topic, Guid payload)
    {
        Topic = topic;
        Payload = payload;
    }
    

    [BsonId]
    public Guid OutboxEventId { get; set; }
    public string Topic { get; set; }
    public Guid Payload { get; set; }
    public bool Sent { get; set; }
}