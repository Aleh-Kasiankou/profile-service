using FluentValidation;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.FluentValidationService.Validators;

public class ProfileValidator : AbstractValidator<Profile>
{
    public ProfileValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
    }
}