namespace BackendSignals.Requests
{
    public class FlightBeginRequest
    {
        public int OperatorID { get; set; }
        public int TeamID { get; set; }
        public long FlightID { get; set; }
        public int PlatoonID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public DateTime BeginTime { get; set; }
        public string? Comment { get; set; }
    }
}
