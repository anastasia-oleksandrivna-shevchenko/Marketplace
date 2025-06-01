using Marketplace.BBL.DTO.Review;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _service;
    public ReviewController(IReviewService service)
    {
        _service = service;
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var reviews = await _service.GetReviewsByProductIdAsync(productId);
        if (reviews.IsNullOrEmpty())
            return NotFound(new { message = $"Reviews with product id {productId} not found." });
        return Ok(reviews);
    }

    [HttpGet("sort-by-rating")]
    public async Task<IActionResult> SortByRating([FromQuery] bool ascending)
    {
        var reviews = await _service.GetReviewsSortedByRatingAsync(ascending);
        return Ok(reviews);
    }

    [HttpGet("sort-by-date")]
    public async Task<IActionResult> SortByDate([FromQuery] bool ascending)
    {
        var reviews = await _service.GetReviewsSortedByDateAsync(ascending);
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _service.GetReviewByIdAsync(id);
        if (review == null)
            return NotFound(new { message = $"Review with id {id} not found." });
        return Ok(review);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _service.GetAllReviewsAsync();
        return Ok(reviews);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var review = await _service.CreateReviewAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateReviewDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service.GetReviewByIdAsync(dto.ReviewId);
        if (existing == null)
            return NotFound(new { message = $"Review with id {dto.ReviewId} not found." });
        
        await _service.UpdateReviewAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetReviewByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Review with id {id} not found." });

        await _service.DeleteReviewAsync(id); 
        return NoContent();
    }
}