using Marketplace.BBL.DTO.Review;
using Marketplace.BBL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return NotFound();
        return Ok(review);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _service.GetAllReviewsAsync();
        return Ok(reviews);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateReviewDto dto)
    {
        var review = await _service.CreateReviewAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateReviewDto dto)
    {
        await _service.UpdateReviewAsync(dto); 
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteReviewAsync(id); 
        return NoContent();
    }
}