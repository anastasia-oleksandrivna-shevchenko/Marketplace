using AutoMapper;
using Marketplace.BBL.DTO.Store;
using Marketplace.BBL.Services.Interfaces;
using Marketplace.DAL.Entities;
using Marketplace.DAL.Repositories.Interfaces;

namespace Marketplace.BBL.Services;

public class StoreService: IStoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<StoreDto>> GetAllStoresAsync()
    {
        var stores = await _unitOfWork.StoreRepository.FindAllAsync();
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }
    
    public async Task<IEnumerable<StoreDto>> GetStoresByUserIdAsync(int userId)
    {
        var stores = await _unitOfWork.StoreRepository.FindStoresByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }

    public async Task<IEnumerable<StoreDto>> GetStoresSortedByOrdersCountAsync(bool ascending = false)
    {
        var stores = await _unitOfWork.StoreRepository.FindStoresSortedByOrdersCountAsync(ascending);
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }

    public async Task<IEnumerable<StoreDto>> GetStoresSortedByRatingAsync(bool ascending = false)
    {
        var stores = await _unitOfWork.StoreRepository.FindStoresSortedByRatingAsync(ascending);
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }

    public async Task<StoreDto> GetStoreByIdAsync(int id)
    {
        var store = await _unitOfWork.StoreRepository.FindByIdAsync(id);
        if (store == null) 
            throw new Exception("Store not found");
        return _mapper.Map<StoreDto>(store);
    }

    public async Task<IEnumerable<StoreDto>> GetStoresByNameAsync(string name)
    {
        var stores = await _unitOfWork.StoreRepository.FindStoresByNameAsync(name);
        if (stores == null) 
            throw new Exception("Stores not found");
        return _mapper.Map<IEnumerable<StoreDto>>(stores);
    }
    
    public async Task<StoreDto> CreateStoreAsync(CreateStoreDto dto)
    {
        var store = _mapper.Map<Store>(dto);
        await _unitOfWork.StoreRepository.CreateAsync(store);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<StoreDto>(store);
    }
    
    public async Task UpdateStoreAsync(UpdateStoreDto dto)
    {
        var store = await _unitOfWork.StoreRepository.FindByIdAsync(dto.StoreId);
        if (store == null) throw new Exception("Store not found");

        store.StoreName = dto.StoreName ?? store.StoreName;
        store.Description = dto.Description ?? store.Description;
        store.Location = dto.Location ?? store.Location;

        await _unitOfWork.SaveAsync();
    }
    
    public async Task DeleteStoreAsync(int storeId)
    {
        var store = await _unitOfWork.StoreRepository.FindByIdAsync(storeId);
        if (store == null) 
            throw new Exception("Store not found");

        _unitOfWork.StoreRepository.Delete(store);
        await _unitOfWork.SaveAsync();
    }
    
}