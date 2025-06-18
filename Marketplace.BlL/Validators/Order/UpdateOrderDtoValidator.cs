using FluentValidation;
using Marketplace.BLL.DTO.Order;

namespace Marketplace.BLL.Validators.Order;

public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderStatusDto>
{
    private static readonly List<string> AllowedStatuses = new() { "Pending", "Shipped", "Completed" };
    
    public UpdateOrderDtoValidator()
    {
        RuleFor(o => o.OrderId)
            .GreaterThan(0)
            .WithMessage("OrderId must be greater than zero.");

        RuleFor(o => o.Status)
            .NotEmpty()
            .WithMessage("Status cannot be empty.")
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage($"Status must be one of the following: {string.Join(", ", AllowedStatuses)}.");

    }
}