using Marketplace.DAL.Data;
using Marketplace.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly MarketplaceDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(MarketplaceDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default) => await _dbSet.ToListAsync(cancellationToken);
    
    public async Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default) => await _dbSet.FindAsync(id, cancellationToken);
    
    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default) => await _dbSet.AddAsync(entity, cancellationToken);
    
    public void Update(T entity, CancellationToken cancellationToken = default) => _dbSet.Update(entity);
    
    public void Delete(T entity, CancellationToken cancellationToken = default) => _dbSet.Remove(entity);
    
}