using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions.ClientRelatedExceptions;

public class ProfileWithSameUsernameAlreadyRegisteredException : ClientRelatedException
{
    static string FormatMessage(string username) => $"The username {username} is already taken. Please pick another username."; 
    public ProfileWithSameUsernameAlreadyRegisteredException(string username) : base(FormatMessage(username))
    {
    }
}