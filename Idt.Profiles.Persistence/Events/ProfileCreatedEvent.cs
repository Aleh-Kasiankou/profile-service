using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Events;

public class ProfileCreatedEvent : ProfileEvent
{
    public ProfileCreatedEvent(Profile profile) : base(profile)
    {
    }
}