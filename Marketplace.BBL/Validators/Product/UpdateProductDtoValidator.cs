using FluentValidation;
using Marketplace.BBL.DTO.Product;

namespace Marketplace.BBL.Validators.Product;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        RuleFor(p => p.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than zero.");

        RuleFor(p => p.Name)
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.")
            .When(p => p.Name != null);

        RuleFor(p => p.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters.")
            .When(p => p.Description != null);

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal to zero.")
            .When(p => p.Price.HasValue);

        RuleFor(p => p.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be greater than or equal to zero.")
            .When(p => p.Quantity.HasValue);

        RuleFor(p => p.ImageUrl)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Invalid image URL format.")
            .When(p => !string.IsNullOrEmpty(p.ImageUrl));

        RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than zero.")
            .When(p => p.CategoryId.HasValue);
    }
}