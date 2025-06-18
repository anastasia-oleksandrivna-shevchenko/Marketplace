using AutoMapper;
using Marketplace.BLL.DTO.OrderItem;
using Marketplace.BLL.Exceptions;
using Marketplace.BLL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.UnitOfWork;

namespace Marketplace.BLL.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto dto, CancellationToken cancellationToken = default)
    {
        var orderItem = _mapper.Map<OrderItem>(dto);
        await _unitOfWork.OrderItemRepository.CreateAsync(orderItem, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        return _mapper.Map<OrderItemDto>(orderItem);
    }
    
    public async Task UpdateOrderItemAsync(UpdateOrderItemDto dto, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.OrderItemRepository.FindByIdAsync(dto.OrderItemId, cancellationToken);
        if (item == null) 
            throw new NotFoundException($"Order item with ID {dto.OrderItemId} not found");
        
        item.Quantity = dto.Quantity;

        await _unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task DeleteOrderItemAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.OrderItemRepository.FindByIdAsync(id, cancellationToken);
        if (item == null) 
            throw new NotFoundException($"Order item with ID {id} not found");

        _unitOfWork.OrderItemRepository.Delete(item, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
    
    public async Task<OrderItemDto> GetOrderItemByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _unitOfWork.OrderItemRepository.FindOrderItemWithProductsByIdAsync(id, cancellationToken);
        if (item == null) 
        throw new NotFoundException($"Order item with ID {id} not found");
        
        return _mapper.Map<OrderItemDto>(item);
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync(CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.OrderItemRepository.FindOrderItemsWithProductsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<OrderItemDto>>(items);
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var items = await _unitOfWork.OrderItemRepository.FindItemsByOrderIdAsync(orderId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderItemDto>>(items);
    }
    
}