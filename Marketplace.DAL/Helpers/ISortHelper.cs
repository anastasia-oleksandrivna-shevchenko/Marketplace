using Marketplace.BBL.DTO.Parameters;
using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Helpers;

public interface ISortHelper<T>
{
    IQueryable<T> ApplySort(IQueryable<T> entities, string? orderByQueryString);
}