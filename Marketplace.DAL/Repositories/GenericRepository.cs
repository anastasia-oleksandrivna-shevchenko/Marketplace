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
    
    public async Task<IEnumerable<T>> FindAllAsync() => await _dbSet.ToListAsync();
    
    public async Task<T> FindByIdAsync(int id) => await _dbSet.FindAsync(id);
    
    public async Task CreateAsync(T entity) => await _dbSet.AddAsync(entity);
    
    public void Update(T entity) => _dbSet.Update(entity);
    
    public void Delete(T entity) => _dbSet.Remove(entity);
    
}