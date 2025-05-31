namespace Marketplace.BBL.DTO.Product;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } 
    public int StoreId { get; set; }
    public string StoreName { get; set; } 
    public string ImageUrl { get; set; }
}