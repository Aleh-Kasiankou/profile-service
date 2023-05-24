using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.MessageBroker.BusEvents;

public abstract class ProfileEvent : BaseBusEvent<Profile>
{
    protected ProfileEvent(Profile profile)
    {
        Body = profile;
    }
    public override Profile Body { get; }
}