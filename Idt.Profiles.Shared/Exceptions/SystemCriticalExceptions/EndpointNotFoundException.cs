using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;

public class EndpointNotFoundException : ServerRelatedException
{
    static string FormatExceptionMessage(string actionName) => $"Failed to generate URI for action {actionName}";

    public EndpointNotFoundException(string actionName) : base(FormatExceptionMessage(actionName))
    {
        
    }
}