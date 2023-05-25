namespace Idt.Profiles.Shared.Exceptions;

public class EndpointNotFoundException : ApplicationRelatedException
{
    static string FormatExceptionMessage(string actionName) => $"Failed to generate URI for action {actionName}";

    public EndpointNotFoundException(string actionName) : base(FormatExceptionMessage(actionName))
    {
        
    }
}