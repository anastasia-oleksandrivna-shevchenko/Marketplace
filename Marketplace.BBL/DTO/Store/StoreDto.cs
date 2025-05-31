namespace Marketplace.BBL.DTO.Store;

public class StoreDto
{
    public int StoreId { get; set; }
    public int UserId { get; set; } 
    public string StoreName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public float Rating { get; set; }
}