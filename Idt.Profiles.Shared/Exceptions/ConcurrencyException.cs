namespace Idt.Profiles.Shared.Exceptions;

public abstract class ConcurrencyException : ApplicationException
{
    protected ConcurrencyException(string formattedMessage): base(formattedMessage)
    {
        
    }
}