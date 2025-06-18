using Marketplace.BLL.DTO.Store;

namespace Marketplace.BLL.Services.Interfaces;

public interface IStoreService
{
    Task <IEnumerable<StoreDto>> GetStoresByUserIdAsync(int userId);
    Task<StoreDto> GetStoreByIdAsync(int storeId);
    Task<IEnumerable<StoreDto>> GetStoresByNameAsync(string name);
    Task<IEnumerable<StoreDto>> GetStoresSortedByOrdersCountAsync(bool ascending = false);
    Task<IEnumerable<StoreDto>> GetStoresSortedByRatingAsync(bool ascending = false);
    Task<StoreDto> CreateStoreAsync(CreateStoreDto dto);
    Task<IEnumerable<StoreDto>> GetAllStoresAsync();
    Task UpdateStoreAsync(UpdateStoreDto dto);
    Task DeleteStoreAsync(int storeId);
}