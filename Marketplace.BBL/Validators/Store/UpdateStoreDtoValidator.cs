using FluentValidation;
using Marketplace.BBL.DTO.Store;

namespace Marketplace.BBL.Validators.Store;

public class UpdateStoreDtoValidator : AbstractValidator<UpdateStoreDto>
{
    public UpdateStoreDtoValidator()
    {
        RuleFor(s => s.StoreId)
            .GreaterThan(0)
            .WithMessage("StoreId must be greater than zero.");
        
        RuleFor(s => s.StoreName)
            .NotEmpty()
            .WithMessage("Store name cannot be empty.")
            .MaximumLength(100)
            .WithMessage("Store name cannot exceed 100 characters.")
            .When(s => s.StoreName != null);

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty.")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(s => s.Description != null);
        
        RuleFor(s => s.Location)
            .NotEmpty()
            .WithMessage("Location cannot be empty.")
            .MaximumLength(200)
            .WithMessage("Location cannot exceed 200 characters.")
            .When(s => s.Location != null);
    }
}