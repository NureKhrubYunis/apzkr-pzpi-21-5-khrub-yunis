using Medoxity.Models;
using Medoxity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Medoxity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArduinoController : ControllerBase
    {
        private readonly IArduinoService _arduinoService;

        public ArduinoController(IArduinoService arduinoService)
        {
            _arduinoService = arduinoService;
        }

        [HttpGet("getdata")]
        public IActionResult GetData()
        {
            var data = _arduinoService.GetSensorData();
            return Ok(data);
        }

        [HttpPost("calculateaveragepulse")]
        public IActionResult CalculateAveragePulse([FromBody] List<SensorData> sensorDataList)
        {
            try
            {
                var averagePulse = _arduinoService.CalculateAveragePulse(sensorDataList);
                return Ok(new { AveragePulse = averagePulse });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("calculateaveragetemperature")]
        public IActionResult CalculateAverageTemperature([FromBody] List<SensorData> sensorDataList)
        {
            try
            {
                var averageTemperature = _arduinoService.CalculateAverageTemperature(sensorDataList);
                return Ok(new { AverageTemperature = averageTemperature });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
