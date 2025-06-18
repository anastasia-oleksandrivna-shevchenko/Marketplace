using FluentValidation;
using Marketplace.BLL.DTO.User;

namespace Marketplace.BLL.Validators.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than zero.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters.")
            .When(x => x.FirstName != null);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters.")
            .When(x => x.LastName != null);

        RuleFor(x => x.MiddleName)
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 50 characters.")
            .When(x => x.MiddleName != null);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.")
            .Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Phone number is not valid.")
            .When(x => x.Phone != null);
    }
}