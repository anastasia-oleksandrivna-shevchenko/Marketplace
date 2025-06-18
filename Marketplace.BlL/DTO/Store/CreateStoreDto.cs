namespace Marketplace.BLL.DTO.Store;

public class CreateStoreDto
{
    public int UserId { get; set; }
    public string StoreName { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
}