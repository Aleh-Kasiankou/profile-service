using Idt.Profiles.Services.MessageBroker.BusEvents;

namespace Idt.Profiles.Services.MessageBroker;

public interface IMessageBroker
{
    Task Publish<T>(T busEvent) where T: BaseBusEvent<T>;
}