using BackendSignals.Services.Implementations;
using System.Text.Json.Serialization;

namespace BackendSignals.Requests
{
    public class MeasurementRequest
    {
        [JsonConverter(typeof(NumberToStringConverter))]
        public string FlightId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, float> Measurements { get; set; } = new Dictionary<string, float>();
    }
}
