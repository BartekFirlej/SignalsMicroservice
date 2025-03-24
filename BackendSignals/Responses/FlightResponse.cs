using BackendSignals.Models;

namespace BackendSignals.Responses
{
    public class FlightResponse
    {
        public string FlightID { get; set; }
        public int OperatorID { get; set; }
        public int TeamID { get; set; }
        public int PlatoonID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Comment { get; set; }
        public string VideoStream { get; set; }
        public List<Measurement> Measurements = new List<Measurement>();

        public FlightResponse(Flight flight)
        {
            FlightID = flight.FlightID.ToString();
            OperatorID = flight.OperatorID;
            TeamID = flight.TeamID;
            PlatoonID = flight.PlatoonID;
            EndTime = flight.EndTime;
            X = flight.X;
            Y = flight.Y;
            Z = flight.Z;
            BeginTime = flight.BeginTime;
            Comment = flight.Comment;
            Measurements = flight.Measurements;
            VideoStream = flight.VideoStream;
        }
    }
}
