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
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService, IUserService userService, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("message")]
        [Authorize]
        public async Task<IActionResult> SendMessage(string receiverUsername, string text)
        {
            var senderUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(senderUsername))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var receiverUser = await _userService.GetCurrentUserAsync(receiverUsername);
            if (receiverUser == null)
            {
                return NotFound(new { message = "Receiver not found." });
            }

            try
            {
                await _messageService.SendMessageAsync(senderUsername, receiverUsername, text);
                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string senderUsername, string receiverUsername)
        {
            if (string.IsNullOrEmpty(senderUsername) || string.IsNullOrEmpty(receiverUsername))
            {
                return BadRequest(new { message = "SenderUsername and ReceiverUsername must be provided." });
            }

            var messages = await _messageService.GetMessagesBetweenUsersAsync(senderUsername, receiverUsername);

            if (messages == null || !messages.Any())
            {
                return NotFound(new { message = "No messages found between the specified users." });
            }

            return Ok(messages);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageService.GetAllMessagesAsync();
            return Ok(messages);
        }
    }
}
