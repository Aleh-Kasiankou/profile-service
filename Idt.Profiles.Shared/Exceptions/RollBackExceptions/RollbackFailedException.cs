using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.RollBackExceptions;

public class RollbackFailedException : ServerRelatedException
{
    public RollbackFailedException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}