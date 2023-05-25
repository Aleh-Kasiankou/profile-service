namespace Idt.Profiles.Persistence.Models.Interfaces;

public interface IModifiableAggregate
{
    public Guid ConcurrencyMarker { get; set; }
}