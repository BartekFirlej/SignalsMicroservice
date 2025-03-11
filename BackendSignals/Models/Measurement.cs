using BackendSignals.Requests;

namespace BackendSignals.Models
{
    public class Measurement
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, float> SignalMeasurements { get; set; } = new Dictionary<string, float>();

        public Measurement(MeasurementRequest measurementRequest)
        {
            X = measurementRequest.X;
            Y = measurementRequest.Y;
            Z = measurementRequest.Z;
            Timestamp = measurementRequest.Timestamp;
            SignalMeasurements = measurementRequest.Measurements;
        }
    }
}
