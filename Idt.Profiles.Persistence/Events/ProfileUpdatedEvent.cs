using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Events;

public class ProfileUpdatedEvent : ProfileEvent
{
    public ProfileUpdatedEvent(Profile profile) : base(profile)
    {
    }
}
