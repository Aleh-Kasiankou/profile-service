using Idt.Profiles.Persistence.Events;

namespace Idt.Profiles.Persistence.Models;

public class OutboxEvent
{
    public OutboxEvent(ProfileEvent profileEvent)
    {
        Event = profileEvent;
    }
    public ProfileEvent Event { get; set; }
    public bool Sent { get; set; }
    public bool Confirmed { get; set; }
}