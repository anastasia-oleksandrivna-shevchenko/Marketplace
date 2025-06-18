using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> FindItemsByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);

    public Task<OrderItem?> FindOrderItemWithProductsByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<OrderItem>> FindOrderItemsWithProductsAsync(CancellationToken cancellationToken = default);

}