using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.FluentValidationService.Validators;

public class ImageFileValidator : AbstractValidator<IFormFile>
{
    public ImageFileValidator()
    {
        RuleFor(x => x.ContentType).Must(x => x.Contains("image"))
            .WithMessage("The file is not recognized as an image. Please try uploading another image.");
        RuleFor(x => x.Length).LessThanOrEqualTo(4194304)
            .WithMessage("The file size should be not larger than 4 mb.");
    }
}