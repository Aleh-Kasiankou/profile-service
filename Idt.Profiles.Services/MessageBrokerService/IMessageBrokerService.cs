using Idt.Profiles.Persistence.Events;

namespace Idt.Profiles.Services.MessageBrokerService;

public interface IMessageBrokerService
{
    Task Publish<T>(T busEvent) where T: BaseBusEvent<T>;
}