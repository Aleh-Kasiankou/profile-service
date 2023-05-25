using Idt.Profiles.Shared.Exceptions.BaseExceptions;

namespace Idt.Profiles.Shared.Exceptions;

public class ProfileConcurrentUpdateFailedException : ConcurrencyException
{
    static string FormatMessage(Guid profileId) => $"Failed to update profile with id {profileId}. Please try updating the information again.";
    public ProfileConcurrentUpdateFailedException(Guid profileId) : base(FormatMessage(profileId))
    {
    }
}