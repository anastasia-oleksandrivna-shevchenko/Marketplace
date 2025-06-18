using FluentValidation;
using Marketplace.BLL.DTO.Category;

namespace Marketplace.BLL.Validators.Category;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(c => c.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than zero.");

        RuleFor(c => c.Name)
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
    }
}