using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.MessageBroker.BusEvents;

public class ProfileDeletedEvent : ProfileEvent
{
    public ProfileDeletedEvent(Profile profile) : base(profile)
    {
    }
}