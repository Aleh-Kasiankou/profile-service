using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;

public class FailedToGetProfileImageFromDriveException : ServerRelatedException
{
    public FailedToGetProfileImageFromDriveException(string formattedExceptionMessage) : base(formattedExceptionMessage)
    {
    }
}