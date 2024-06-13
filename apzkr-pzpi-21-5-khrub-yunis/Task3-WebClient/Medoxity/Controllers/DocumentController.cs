using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medoxity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("user-documents")]
        public async Task<IActionResult> GetUserDocuments()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var documents = await _documentService.GetDocumentsByUsernameAsync(username);

            if (documents == null || !documents.Any())
            {
                return NotFound(new { message = "No documents found for the specified user." });
            }

            return Ok(documents);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }
    }
}
