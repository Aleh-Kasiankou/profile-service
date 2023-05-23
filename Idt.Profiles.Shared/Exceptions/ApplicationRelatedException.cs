namespace Idt.Profiles.Shared.Exceptions;

public abstract class ApplicationRelatedException : ApplicationException
{
    protected ApplicationRelatedException(string formatExceptionMessage) : base(formatExceptionMessage)
    {
    }
}