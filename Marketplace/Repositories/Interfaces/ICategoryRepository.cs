using Marketplace.Entities;

namespace Marketplace.Repositories.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IEnumerable<Category>> GetCategoriesSortedByNameAsync(bool ascending);

}