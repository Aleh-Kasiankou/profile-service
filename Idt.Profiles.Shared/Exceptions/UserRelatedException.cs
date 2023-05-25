namespace Idt.Profiles.Shared.Exceptions;

public abstract class UserRelatedException : ApplicationException
{
    public UserRelatedException(string formattedMessage) : base(formattedMessage)
    {
    }
}