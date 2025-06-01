namespace Marketplace.DAL.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync();
    Task<T> FindByIdAsync(int id);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}