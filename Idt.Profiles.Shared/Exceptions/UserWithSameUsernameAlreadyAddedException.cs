namespace Idt.Profiles.Shared.Exceptions;

public class ProfileWithSameUsernameAlreadyRegisteredException : UserRelatedException
{
    static string FormatMessage(string username) => $"The username {username} is already taken. Please pick another username."; 
    public ProfileWithSameUsernameAlreadyRegisteredException(string username) : base(FormatMessage(username))
    {
    }
}