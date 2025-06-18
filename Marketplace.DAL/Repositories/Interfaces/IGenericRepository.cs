namespace Marketplace.DAL.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity, CancellationToken cancellationToken = default);
    void Delete(T entity, CancellationToken cancellationToken = default);
}