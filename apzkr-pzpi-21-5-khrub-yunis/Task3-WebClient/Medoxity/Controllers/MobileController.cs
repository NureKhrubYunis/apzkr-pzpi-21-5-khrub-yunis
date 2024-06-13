using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medoxity.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MobileController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IDrugService _drugService;
        private readonly IDocumentService _documentService;
        private readonly IMessageService _messageService;
        private readonly ILogger<CommentsController> _logger;

        public MobileController(ICommentService commentService, IDrugService drugService, IDocumentService documentService, IMessageService messageService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _drugService = drugService;
            _documentService = documentService;
            _messageService = messageService;
            _logger = logger;
        }

        [HttpGet("drugs-all")]
        public async Task<IActionResult> GetAllDrugs()
        {
            var drugs = await _drugService.GetAllDrugsAsync();
            return Ok(drugs);
        }


        [HttpGet("comments-all")]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("documents-all")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("users-with-messages/{username}")]
        public async Task<IActionResult> GetUsersWithMessages(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { message = "Username must be provided." });
            }

            var usersWithLastMessages = await _messageService.GetUsersWithMessagesAsync(username);
            if (usersWithLastMessages == null)
            {
                return NotFound(new { message = "No messages found for this user." });
            }

            var result = usersWithLastMessages.Select(uwlm => new
            {
                User = uwlm.User,
                LastMessage = uwlm.LastMessage.Text,
                SentDate = uwlm.LastMessage.SentDate
            });

            return Ok(result);
        }

        [HttpGet("messages/{senderUsername}/{receiverUsername}")]
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

    }
}
