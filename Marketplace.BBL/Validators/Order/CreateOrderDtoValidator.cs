using FluentValidation;
using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.Validators.OrderItem;

namespace Marketplace.BBL.Validators.Order;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(o => o.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than zero.");

        RuleFor(o => o.StoreId)
            .GreaterThan(0)
            .WithMessage("StoreId must be greater than zero.");

        RuleFor(o => o.OrderItems)
            .NotNull()
            .WithMessage("OrderItems cannot be null.")
            .Must(items => items.Count > 0)
            .WithMessage("Order must contain at least one order item.");

        RuleForEach(o => o.OrderItems).SetValidator(new CreateOrderItemDtoValidator());
    }
}