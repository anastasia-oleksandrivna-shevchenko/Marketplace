using Marketplace.BLL.DTO.Review;
using Marketplace.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var reviews = await _service.GetReviewsByProductIdAsync(productId);
        if (reviews.IsNullOrEmpty())
            return NotFound(new { message = $"Reviews with product id {productId} not found." });
        return Ok(reviews);
    }

    [HttpGet("sort-by-rating")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByRating([FromQuery] bool ascending)
    {
        var reviews = await _service.GetReviewsSortedByRatingAsync(ascending);
        return Ok(reviews);
    }

    [HttpGet("sort-by-date")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SortByDate([FromQuery] bool ascending)
    {
        var reviews = await _service.GetReviewsSortedByDateAsync(ascending);
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _service.GetReviewByIdAsync(id);
        if (review == null)
            return NotFound(new { message = $"Review with id {id} not found." });
        return Ok(review);
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _service.GetAllReviewsAsync();
        return Ok(reviews);
    }

    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
    {
        var review = await _service.CreateReviewAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
    }

    [Authorize]
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromBody] UpdateReviewDto dto)
    {
        var existing = await _service.GetReviewByIdAsync(dto.ReviewId);
        if (existing == null)
            return NotFound(new { message = $"Review with id {dto.ReviewId} not found." });
        
        await _service.UpdateReviewAsync(dto); 
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetReviewByIdAsync(id);
        if (existing == null)
            return NotFound(new { message = $"Review with id {id} not found." });

        await _service.DeleteReviewAsync(id); 
        return NoContent();
    }
}