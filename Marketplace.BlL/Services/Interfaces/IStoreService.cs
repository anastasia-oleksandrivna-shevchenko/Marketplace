using Marketplace.BLL.DTO.Store;

namespace Marketplace.BLL.Services.Interfaces;

public interface IStoreService
{
    Task <IEnumerable<StoreDto>> GetStoresByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<StoreDto> GetStoreByIdAsync(int storeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreDto>> GetStoresByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreDto>> GetStoresSortedByOrdersCountAsync(bool ascending = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreDto>> GetStoresSortedByRatingAsync(bool ascending = false, CancellationToken cancellationToken = default);
    Task<StoreDto> CreateStoreAsync(CreateStoreDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<StoreDto>> GetAllStoresAsync(CancellationToken cancellationToken = default);
    Task UpdateStoreAsync(UpdateStoreDto dto, CancellationToken cancellationToken = default);
    Task DeleteStoreAsync(int storeId, CancellationToken cancellationToken = default);
}