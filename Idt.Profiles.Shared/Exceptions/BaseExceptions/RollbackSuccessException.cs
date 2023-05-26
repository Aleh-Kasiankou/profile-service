namespace Idt.Profiles.Shared.Exceptions.BaseExceptions;

public class RollbackSuccessException : ServerRelatedException
{
    public RollbackSuccessException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}