using AutoMapper;
using Marketplace.BBL.DTO.OrderItem;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.BBL.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<OrderItemDto> CreateOrderItemAsync(CreateOrderItemDto dto)
    {
        var orderItem = _mapper.Map<OrderItem>(dto);
        await _unitOfWork.OrderItemRepository.CreateAsync(orderItem);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<OrderItemDto>(orderItem);
    }
    
    public async Task UpdateOrderItemAsync(UpdateOrderItemDto dto)
    {
        var item = await _unitOfWork.OrderItemRepository.FindByIdAsync(dto.OrderItemId);
        if (item == null) 
            throw new Exception("Order item not found");
        
        item.Quantity = dto.Quantity ?? item.Quantity;

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteOrderItemAsync(int id)
    {
        var item = await _unitOfWork.OrderItemRepository.FindByIdAsync(id);
        if (item == null) 
            throw new Exception("Order item not found");

        _unitOfWork.OrderItemRepository.Delete(item);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<OrderItemDto> GetOrderItemByIdAsync(int id)
    {
        var item = await _unitOfWork.OrderItemRepository.FindByIdAsync(id);
        if (item == null) 
            throw new Exception("Order item not found");
        return _mapper.Map<OrderItemDto>(item);
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetAllOrderItemsAsync()
    {
        var items = await _unitOfWork.OrderItemRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<OrderItemDto>>(items);
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetItemsByOrderIdAsync(int orderId)
    {
        var items = await _unitOfWork.OrderItemRepository.FindItemsByOrderIdAsync(orderId);
        return _mapper.Map<IEnumerable<OrderItemDto>>(items);
    }
    
}