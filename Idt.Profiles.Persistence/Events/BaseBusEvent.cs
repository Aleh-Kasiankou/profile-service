namespace Idt.Profiles.Persistence.Events;

public abstract class BaseBusEvent<T>
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime IssuedAt { get; } = DateTime.Now;
    public TimeSpan TimeToLive { get; } = TimeSpan.FromMinutes(20);
    public abstract T Body { get; }
}