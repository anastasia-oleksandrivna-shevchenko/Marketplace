using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Repositories.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> FindCategoriesSortedByNameAsync(bool ascending, CancellationToken cancellationToken = default);

}