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
    [Route("api/[controller]")]
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
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var senderUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(senderUsername))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var receiverUser = await _userService.GetCurrentUserAsync(request.ReceiverUsername);
            if (receiverUser == null)
            {
                return NotFound(new { message = "Receiver not found." });
            }

            try
            {
                await _messageService.SendMessageAsync(senderUsername, request.ReceiverUsername, request.Text);
                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("messages/{receiverUsername}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesBetweenUsers(string receiverUsername)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(receiverUsername))
            {
                return BadRequest(new { message = "SenderUsername and ReceiverUsername must be provided." });
            }

            var messages = await _messageService.GetMessagesBetweenUsersAsync(username, receiverUsername);

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

        [HttpGet("users-with-messages")]
        public async Task<IActionResult> GetUsersWithMessages()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var usersWithLastMessages = await _messageService.GetUsersWithMessagesAsync(username);
            var result = usersWithLastMessages.Select(uwlm => new
            {
                User = uwlm.User,
                LastMessage = uwlm.LastMessage.Text,
                SentDate = uwlm.LastMessage.SentDate
            });

            return Ok(result);
        }
    }
}
