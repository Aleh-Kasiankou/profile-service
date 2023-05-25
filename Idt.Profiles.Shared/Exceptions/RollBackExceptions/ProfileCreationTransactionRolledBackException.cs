using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.RollBackExceptions;

public class ProfileCreationTransactionRolledBackException : ServerRelatedException
{
    private const string DefaultMessage =
        "Profile creation failed. Please try again in a few minutes or contact our support team.";

    public ProfileCreationTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}