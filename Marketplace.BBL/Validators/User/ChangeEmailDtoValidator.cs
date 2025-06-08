using FluentValidation;
using Marketplace.BBL.DTO.User;

namespace Marketplace.BBL.Validators.User;

public class ChangeEmailDtoValidator  : AbstractValidator<ChangeEmailDto>
{
    public ChangeEmailDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than zero.");

        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .WithMessage("New email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");
    }
}