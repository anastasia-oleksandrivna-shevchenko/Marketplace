﻿using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository CategoryRepository { get; }
    IOrderRepository OrderRepository { get; }
    IOrderItemRepository OrderItemRepository { get; }
    IProductRepository ProductRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IStoreRepository StoreRepository { get; }
    IUserRepository UserRepository { get; }
    
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
    
}