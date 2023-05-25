namespace Idt.Profiles.Shared.Exceptions;

public class ProfileCreationTransactionRolledBackException : ApplicationRelatedException
{
    private const string DefaultMessage =
        "Profile creation failed. Please try again in a few minutes or contact out support team.";

    public ProfileCreationTransactionRolledBackException() : base(DefaultMessage)
    {
    }
}