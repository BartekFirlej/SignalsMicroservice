namespace BackendSignals.Responses
{
    public class MeasurementResponse
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<float, float> Measurements { get; set; } = new Dictionary<float, float>();
    }
}
