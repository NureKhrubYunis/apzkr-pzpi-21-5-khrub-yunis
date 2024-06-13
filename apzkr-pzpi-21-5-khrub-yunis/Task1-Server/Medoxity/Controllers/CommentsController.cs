using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Medoxity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IDrugService _drugService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentService commentService, IDrugService drugService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _drugService = drugService;
            _logger = logger;
        }

        [HttpPost("comment")]
        [Authorize]
        public async Task<IActionResult> AddComment(int drugID, string text)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            try
            {
                var drug = await _drugService.GetDrugByIdAsync(drugID);
                if (drug == null)
                {
                    throw new Exception("Drug not found.");
                }

                await _commentService.AddCommentAsync(username, drugID, text);
                return Ok(new { message = "Comment added successfully." });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("comments/drug/{drugID}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByDrugID(int drugID)
        {
            var comments = await _commentService.GetCommentsByDrugIDAsync(drugID);

            if (comments == null || !comments.Any())
            {
                return NotFound(new { message = "No comments found for this DrugID." });
            }

            return Ok(comments);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }
    }
}
