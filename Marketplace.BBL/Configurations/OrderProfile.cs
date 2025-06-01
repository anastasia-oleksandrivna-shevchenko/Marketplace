using AutoMapper;
using Marketplace.BBL.DTO.Order;
using Marketplace.DAL.Entities;

namespace Marketplace.BBL.Configurations;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderStatusDto, Order>();
    }
}