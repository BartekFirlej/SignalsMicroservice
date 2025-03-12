namespace BackendSignals.Requests
{
    public class FlightEndRequest
    {
        public long FlightID { get; set; }
        public DateTime EndTime { get; set; }
    }
}
