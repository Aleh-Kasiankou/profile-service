namespace Idt.Profiles.Shared.Exceptions.BaseExceptions;

public abstract class ConcurrencyException : ServerRelatedException
{
    protected ConcurrencyException(string formattedMessage): base(formattedMessage)
    {
        
    }
}