namespace BackendSignals.Responses
{
    public class FilghtResponse
    {
        public string FlightID { get; set; }
        public int OperatorID { get; set; }
        public int TeamID { get; set; }
        public int PlatoonID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public DateTime BeginTime { get; set; }
        public string? Comment { get; set; }
        public List<MeasurementResponse> Measurements = new List<MeasurementResponse>();
    }
}
