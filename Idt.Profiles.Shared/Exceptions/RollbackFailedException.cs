namespace Idt.Profiles.Shared.Exceptions;

public class RollbackFailedException : ApplicationRelatedException
{
    public RollbackFailedException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}