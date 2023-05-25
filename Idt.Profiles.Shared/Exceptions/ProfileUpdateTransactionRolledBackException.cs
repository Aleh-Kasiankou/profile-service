namespace Idt.Profiles.Shared.Exceptions;

public class ProfileUpdateTransactionRolledBackException : ApplicationRelatedException
{
    private const string DefaultMessage =
        "Profile update failed. Please try again in a few minutes or contact our support team.";
    public ProfileUpdateTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}