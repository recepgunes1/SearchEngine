using FluentValidation;
using Shared.DTOs;

namespace UrlRegister.Infrastructure.Validators;

public class RegisterUrlDtoValidator : AbstractValidator<RegisterUrlDto>
{
    public RegisterUrlDtoValidator()
    {
        RuleFor(p => p.Link).NotNull().WithMessage("Url can not be null");
        RuleFor(p => p.Link).NotEmpty().WithMessage("Url can not be empty");
        RuleFor(p => p.Link).Must(IsUrlValid).WithMessage("Url format is not valid");
    }

    private bool IsUrlValid(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}