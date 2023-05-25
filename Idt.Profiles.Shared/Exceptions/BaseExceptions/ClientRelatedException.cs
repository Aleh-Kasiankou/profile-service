namespace Idt.Profiles.Shared.Exceptions.BaseExceptions;

public abstract class ClientRelatedException : ApplicationException
{
    public ClientRelatedException(string formattedMessage) : base(formattedMessage)
    {
    }
}