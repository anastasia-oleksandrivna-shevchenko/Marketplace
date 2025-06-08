using FluentValidation;
using Marketplace.BBL.DTO.User;

namespace Marketplace.BBL.Validators.User;

public class ChangeUsernameDtoValidator : AbstractValidator<ChangeUsernameDto>
{
    public ChangeUsernameDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than zero.");

        RuleFor(x => x.NewUsername)
            .NotEmpty()
            .WithMessage("New username is required.")
            .MaximumLength(50)
            .WithMessage("New username cannot exceed 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}