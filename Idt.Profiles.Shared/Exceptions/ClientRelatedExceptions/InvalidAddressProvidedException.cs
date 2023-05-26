using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.ClientRelatedExceptions;

public class InvalidAddressProvidedException : ClientRelatedException
{
    public InvalidAddressProvidedException(string formattedMessage) : base(formattedMessage)
    {
    }
}