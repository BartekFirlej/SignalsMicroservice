namespace BackendSignals.Requests
{
    public class SignalRequest
    {
        public int FlightId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<float, float> Measurements { get; set; } = new Dictionary<float, float>();
    }
}
