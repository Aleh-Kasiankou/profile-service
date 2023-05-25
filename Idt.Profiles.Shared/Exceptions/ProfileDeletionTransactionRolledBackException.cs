namespace Idt.Profiles.Shared.Exceptions;

public class ProfileDeletionTransactionRolledBackException : ApplicationRelatedException
{
    private const string DefaultMessage =
        "Profile deletion failed. Please try again in a few minutes or contact our support team.";
    public ProfileDeletionTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}