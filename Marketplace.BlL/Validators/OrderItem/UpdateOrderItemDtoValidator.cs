using FluentValidation;
using Marketplace.BLL.DTO.OrderItem;

namespace Marketplace.BLL.Validators.OrderItem;

public class UpdateOrderItemDtoValidator : AbstractValidator<UpdateOrderItemDto>
{
    public UpdateOrderItemDtoValidator()
    {
        RuleFor(oi => oi.OrderItemId)
            .GreaterThan(0)
            .WithMessage("OrderItemId must be greater than zero.");
        
        RuleFor(oi => oi.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}