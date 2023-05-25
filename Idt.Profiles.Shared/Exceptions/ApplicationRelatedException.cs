namespace Idt.Profiles.Shared.Exceptions;

public abstract class ApplicationRelatedException : ApplicationException
{
    protected ApplicationRelatedException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}