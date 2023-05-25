using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;

public class EventSyncJobNotRegisteredException : ServerRelatedException
{
    private const string DefaultMessage = "The hangfire job for event publishing failed to initiate";
    public EventSyncJobNotRegisteredException() : base(DefaultMessage)
    {
    }
}