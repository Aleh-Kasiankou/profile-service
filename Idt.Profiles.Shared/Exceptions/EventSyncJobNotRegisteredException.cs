namespace Idt.Profiles.Shared.Exceptions;

public class EventSyncJobNotRegisteredException : ApplicationRelatedException
{
    private const string DefaultMessage = "The hangfire job for event publishing failed to initiate";
    public EventSyncJobNotRegisteredException() : base(DefaultMessage)
    {
    }
}