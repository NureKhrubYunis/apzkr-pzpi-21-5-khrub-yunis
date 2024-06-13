using Medoxity.Models;

namespace Medoxity.Services
{
    public interface IArduinoService
    {
        SensorData GetSensorData();
        double CalculateAveragePulse(List<SensorData> data);
        double CalculateAverageTemperature(List<SensorData> data);
    }
}
