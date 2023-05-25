namespace Idt.Profiles.Shared.Exceptions;

public class ProfileDoesNotExistException : UserRelatedException
{
    private static string FormatMessage(Guid profileId) =>
        $"The user profile with the provided id does not exist : {profileId.ToString()}";

    public ProfileDoesNotExistException(Guid profileId) : base(FormatMessage(profileId))
    {
    }
}