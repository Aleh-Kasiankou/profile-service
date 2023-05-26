using FluentValidation;
using Idt.Profiles.Dto.Dto;

namespace Idt.Profiles.Services.FluentValidationServices.Validators;

public class DummyAddressValidator : AbstractValidator<ProfileAddressCreateUpdateDto>
{
    public DummyAddressValidator()
    {
        RuleFor(x => x.Building)
            .NotEqual(0)
            .WithMessage("Building number cannot be 0. Building count starts from 1");
        RuleFor(x => x.Street)
            .NotEmpty();
        RuleFor(x => x.City)
            .NotEmpty();
        RuleFor(x => x.ZipCode.Length)
            .GreaterThanOrEqualTo(5)
            .WithMessage("The zip code is too short. It should be at least 5 characters long");
        RuleFor(x => x.State)
            .NotEmpty();
        RuleFor(x => x.CountryCode)
            .Length(2)
            .WithMessage(
                "The country code is invalid. ISO 3166 Alpha-2 compatible country code must be exactly 2 characters long.");
        RuleFor(x => x)
            .MustAsync(async (address, cancellationToken) =>
        {
            //pseudocode for calling 3rd party service
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(150);
            var thirdPartyApiValidationResult = true;
            return thirdPartyApiValidationResult;
        }).WithMessage("Address validation failed. Please provide the correct address.");
    }
}