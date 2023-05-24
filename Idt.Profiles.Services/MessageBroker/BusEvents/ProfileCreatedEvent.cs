using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.MessageBroker.BusEvents;

public class ProfileCreatedEvent : ProfileEvent
{
    public ProfileCreatedEvent(Profile profile) : base(profile)
    {
    }
}