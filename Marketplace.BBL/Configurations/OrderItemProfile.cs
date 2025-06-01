using AutoMapper;
using Marketplace.BBL.DTO.OrderItem;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
    }
}