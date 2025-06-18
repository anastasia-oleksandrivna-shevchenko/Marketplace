using AutoMapper;
using Marketplace.BLL.DTO.Order;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BLL.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
    {
        var productIds = dto.OrderItems.Select(i => i.ProductId).ToList();
        var products = await _unitOfWork.ProductRepository.FindByIdsAsync(productIds, cancellationToken);

        if (products.Count != productIds.Count)
            throw new NotFoundException("One or more products not found!");
        
        foreach (var item in dto.OrderItems)
        {
            var product = products.First(p => p.ProductId == item.ProductId);
            if (product.Quantity < item.Quantity)
                throw new InvalidOperationException($"Insufficient quantity of product '{product.Name}' in stock");
        }  
        foreach (var item in dto.OrderItems)
        {
            var product = products.First(p => p.ProductId == item.ProductId);
            product.Quantity -= item.Quantity;
        }
                
        var order = _mapper.Map<Order>(dto);
        await _unitOfWork.OrderRepository.CreateAsync(order, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.OrderRepository.FindOrdersByIdWithUserAndStoreAsync(id, cancellationToken);
        if (order == null) 
            throw new NotFoundException($"Order with ID {id} not found");
        
        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.OrderRepository.FindOrdersWithUserAndStoreAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.OrderRepository.FindOrdersByCustomerIdAsync(customerId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByStoreIdAsync(int storeId, CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.OrderRepository.FindOrdersByStoreIdAsync(storeId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task UpdateOrderAsync(UpdateOrderStatusDto dto, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(dto.OrderId, cancellationToken);
        if (order == null) 
            throw new NotFoundException($"Order with ID {dto.OrderId} not found");

        order.Status = dto.Status ?? order.Status;
        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.OrderRepository.FindByIdAsync(id, cancellationToken);
        if (order == null) 
            throw new NotFoundException($"Order with ID {id} not found");

        _unitOfWork.OrderRepository.Delete(order, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
    
}