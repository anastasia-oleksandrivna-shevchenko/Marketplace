using FluentValidation;
using Marketplace.BBL.DTO.Review;

namespace Marketplace.BBL.Validators.Review;

public class UpdateReviewDtoValidator : AbstractValidator<UpdateReviewDto>
{
    public UpdateReviewDtoValidator()
    {
        RuleFor(r => r.ReviewId)
            .GreaterThan(0)
            .WithMessage("ReviewId must be greater than zero.");
        
        RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.")
            .When(p => p.Rating != null);
        
        RuleFor(r => r.Comment)
            .MaximumLength(500)
            .WithMessage("Comment cannot exceed 500 characters.");
    
    }
}