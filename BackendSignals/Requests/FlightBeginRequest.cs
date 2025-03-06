namespace BackendSignals.Requests
{
    public class FlightBeginRequest
    {
        public int OperatorID { get; set; }
        public int TeamID { get; set; }
        public int FlightID { get; set; }
        public int PlatoonID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public DateTime BeginTime { get; set; }
        public string? Comment { get; set; }
    }
}
