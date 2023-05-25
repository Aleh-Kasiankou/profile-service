namespace Idt.Profiles.Shared.Exceptions.BaseExceptions;

public abstract class ServerRelatedException : ApplicationException
{
    protected ServerRelatedException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}