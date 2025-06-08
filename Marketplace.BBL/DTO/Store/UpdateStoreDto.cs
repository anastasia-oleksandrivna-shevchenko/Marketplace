namespace Marketplace.BBL.DTO.Store;

public class UpdateStoreDto
{
    public int StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
}