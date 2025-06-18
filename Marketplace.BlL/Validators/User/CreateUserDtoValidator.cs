using FluentValidation;
using Marketplace.BLL.DTO.User;

namespace Marketplace.BLL.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 100 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(u => u.MiddleName)
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 100 characters.")
            .When(u => !string.IsNullOrEmpty(u.MiddleName));

        RuleFor(u => u.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MaximumLength(50)
            .WithMessage("Username cannot exceed 50 characters.");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters.");

        RuleFor(u => u.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Invalid phone number format.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(r => new[] { "Buyer", "Seller", "Admin" }.Contains(r))
            .WithMessage("Role must be either 'Buyer', 'Seller', 'Admin'.");
    }
}