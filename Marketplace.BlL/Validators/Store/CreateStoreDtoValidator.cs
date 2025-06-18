using FluentValidation;
using Marketplace.BLL.DTO.Store;

namespace Marketplace.BLL.Validators.Store;

public class CreateStoreDtoValidator : AbstractValidator<CreateStoreDto>
{
    public CreateStoreDtoValidator()
    {
        RuleFor(s => s.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than zero.");

        RuleFor(s => s.StoreName)
            .NotEmpty()
            .WithMessage("Store name is required.")
            .MaximumLength(100)
            .WithMessage("Store name cannot exceed 100 characters.");

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(s => s.Location)
            .NotEmpty()
            .WithMessage("Location is required.")
            .MaximumLength(200)
            .WithMessage("Location cannot exceed 200 characters.");
    }
}