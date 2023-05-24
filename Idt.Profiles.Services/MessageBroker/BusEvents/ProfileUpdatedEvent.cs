using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.MessageBroker.BusEvents;

public class ProfileUpdatedEvent : ProfileEvent
{
    public ProfileUpdatedEvent(Profile profile) : base(profile)
    {
    }
}

