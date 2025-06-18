using AutoMapper;
using Marketplace.BLL.DTO.OrderItem;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
    }
}