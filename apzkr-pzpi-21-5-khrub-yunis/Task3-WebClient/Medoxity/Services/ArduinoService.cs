using Medoxity.Models;

namespace Medoxity.Services
{

    public class ArduinoService : IArduinoService
    {
        private static Random _random = new Random();

        public SensorData GetSensorData()
        {
            
            return new SensorData
            {
                Pulse = _random.Next(60, 100), 
                Temperature = (float)(_random.NextDouble() * 2 + 36), 
                Timestamp = DateTime.UtcNow
            };
        }

        public double CalculateAveragePulse(List<SensorData> data)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data cannot be null or empty.");

            return data.Average(d => d.Pulse);
        }

        public double CalculateAverageTemperature(List<SensorData> data)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data cannot be null or empty.");

            return data.Average(d => d.Temperature);
        }
    }
}
