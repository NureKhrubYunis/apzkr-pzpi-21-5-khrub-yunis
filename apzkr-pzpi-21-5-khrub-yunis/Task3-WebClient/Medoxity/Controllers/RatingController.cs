using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost("rate")]
        public async Task<IActionResult> AddRating([FromBody] AddRatingRequest request)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            if (request.Value < 1 || request.Value > 5)
            {
                return BadRequest(new { message = "Rating value must be between 1 and 5." });
            }

            try
            {
                await _ratingService.AddRatingAsync(username, request.DrugID, request.Value);
                return Ok(new { message = "Rating added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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

        [HttpGet("check-rating/{drugId}")]
        public async Task<IActionResult> CheckRating(int drugId)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var ratingInfo = await _ratingService.CheckUserRatingForDrugAsync(username, drugId);

            if (ratingInfo.Exists)
            {
                return Ok(ratingInfo.Rating);
            }
            else
            {
                return Ok(new { message = "Rating not found." });
            }
        }
    }

}
