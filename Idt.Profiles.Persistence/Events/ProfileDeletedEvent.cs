using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Events;

public class ProfileDeletedEvent : ProfileEvent
{
    public ProfileDeletedEvent(Profile profile) : base(profile)
    {
    }
}