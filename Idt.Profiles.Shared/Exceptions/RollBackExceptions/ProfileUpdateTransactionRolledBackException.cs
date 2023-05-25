using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.RollBackExceptions;

public class ProfileUpdateTransactionRolledBackException : ServerRelatedException
{
    private const string DefaultMessage =
        "Profile update failed. Please try again in a few minutes or contact our support team.";
    public ProfileUpdateTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}