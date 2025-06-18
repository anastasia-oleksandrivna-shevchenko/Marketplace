namespace Marketplace.BLL.DTO.Review;

public class CreateReviewDto
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}