using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medoxity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DrugsController : ControllerBase
    {
        private readonly IDrugService _drugService;
        private readonly ILogger<DrugsController> _logger;

        public DrugsController(IDrugService drugService, ILogger<DrugsController> logger)
        {
            _drugService = drugService;
            _logger = logger;
        }

        // /drugs/{drugID} (GET)
        [HttpGet("drugs/{drugID}")]
        public async Task<IActionResult> GetDrug(int drugID)
        {
            var drug = await _drugService.GetDrugByIdAsync(drugID);
            if (drug == null)
                return NotFound();

            return Ok(drug);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDrugs()
        {
            var drugs = await _drugService.GetAllDrugsAsync();
            return Ok(drugs);
        }

    }
}
