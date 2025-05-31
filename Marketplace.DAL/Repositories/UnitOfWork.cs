using Marketplace.DAL.Data;
using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.DAL.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly MarketplaceDbContext _context;
    
    public ICategoryRepository CategoryRepository { get; private set; }
    public IOrderRepository OrderRepository { get; private set; }
    public IOrderItemRepository OrderItemRepository { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    public IReviewRepository ReviewRepository { get; private set; }
    public IStoreRepository StoreRepository { get; private set; }
    public IUserRepository UserRepository { get; private set; }

    public UnitOfWork(MarketplaceDbContext context,
        ICategoryRepository categoryRepository,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IProductRepository productRepository,
        IReviewRepository reviewRepository,
        IStoreRepository storeRepository,
        IUserRepository userRepository)
    {
        _context = context;
        CategoryRepository = categoryRepository;
        OrderRepository = orderRepository;
        OrderItemRepository = orderItemRepository;
        ProductRepository = productRepository;
        ReviewRepository = reviewRepository;
        StoreRepository = storeRepository;
        UserRepository = userRepository;
    }
    
    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    
    public void Dispose() => _context.Dispose();
    
}