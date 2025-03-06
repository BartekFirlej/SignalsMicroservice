namespace BackendSignals.Models
{
    public class Measurement
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<float, float> SignalMeasurements { get; set; } = new Dictionary<float, float>();
    }
}
