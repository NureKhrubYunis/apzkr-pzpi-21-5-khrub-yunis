using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medoxity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRatings()
        {
            var ratings = await _ratingService.GetAllRatingsAsync();
            return Ok(ratings);
        }

        [HttpGet("{ratingId}")]
        public async Task<IActionResult> GetRatingById(int ratingId)
        {
            var rating = await _ratingService.GetRatingByIdAsync(ratingId);
            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
        }

        [HttpPut("{ratingId}")]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] Rating updatedRating)
        {
            if (ratingId != updatedRating.RatingID)
            {
                return BadRequest("Rating ID mismatch.");
            }

            try
            {
                await _ratingService.UpdateRatingAsync(updatedRating);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                await _ratingService.DeleteRatingAsync(ratingId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
