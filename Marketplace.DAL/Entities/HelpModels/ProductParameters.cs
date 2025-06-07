using Marketplace.DAL.Entities;

namespace Marketplace.DAL.Entities.HelpModels;

public class ProductParameters : QueryStringParameters
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? CategoryId { get; set; }
    public int? StoreId { get; set; }
    
}