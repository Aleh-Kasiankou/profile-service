using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.RollBackExceptions;

public class ProfileDeletionTransactionRolledBackException : ServerRelatedException
{
    private const string DefaultMessage =
        "Profile deletion failed. Please try again in a few minutes or contact our support team.";
    public ProfileDeletionTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}