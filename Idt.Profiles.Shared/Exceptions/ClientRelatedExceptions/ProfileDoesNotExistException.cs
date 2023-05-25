using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.ClientRelatedExceptions;

public class ProfileDoesNotExistException : ClientRelatedException
{
    private static string FormatMessage(Guid profileId) =>
        $"The user profile with the provided id does not exist : {profileId.ToString()}";

    public ProfileDoesNotExistException(Guid profileId) : base(FormatMessage(profileId))
    {
    }
}