using AutoMapper;
using Marketplace.BLL.DTO.Order;
using Marketplace.DAL.Entities;

namespace Marketplace.BLL.Configurations;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderStatusDto, Order>();
    }
}