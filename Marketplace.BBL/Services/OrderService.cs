using AutoMapper;
using Marketplace.BBL.DTO.Order;
using Marketplace.BBL.Exceptions;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.BBL.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
    {
        var order = _mapper.Map<Order>(dto);
        await _unitOfWork.OrderRepository.CreateAsync(order);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(id);
        if (order == null) 
            throw new NotFoundException($"Order with ID {id} not found");
        
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _unitOfWork.OrderRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId)
    {
        var orders = await _unitOfWork.OrderRepository.FindOrdersByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByStoreIdAsync(int storeId)
    {
        var orders = await _unitOfWork.OrderRepository.FindOrdersByStoreIdAsync(storeId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderAsync(UpdateOrderStatusDto dto)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(dto.OrderId);
        if (order == null) 
            throw new NotFoundException($"Order with ID {dto.OrderId} not found");

        order.Status = dto.Status ?? order.Status;
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(id);
        if (order == null) 
            throw new NotFoundException($"Order with ID {id} not found");

        _unitOfWork.OrderRepository.Delete(order);
        await _unitOfWork.SaveAsync();
    }
    
}