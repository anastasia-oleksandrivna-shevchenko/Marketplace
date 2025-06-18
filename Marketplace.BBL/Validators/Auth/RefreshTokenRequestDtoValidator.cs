using FluentValidation;
using Marketplace.BBL.DTO.Auth;

namespace Marketplace.BBL.Validators.Auth;

public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .MinimumLength(20).WithMessage("Refresh token must be at least 20 characters long.");
    }
}