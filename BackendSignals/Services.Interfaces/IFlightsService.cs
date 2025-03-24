using BackendSignals.Models;
using BackendSignals.Requests;
using BackendSignals.Responses;

namespace BackendSignals.Services.Interfaces
{
    public interface IFlightsService
    {
        public Task<List<FlightResponse>> GetFlights();
        public Task<FlightResponse> GetFlightByFlightID(long flightID);
        public Task<List<FlightPositionResponse>> GetFlightPositions();
        public Task<Flight> CreateFlight(FlightBeginRequest request);
        public Task<Flight> EndFlight(FlightEndRequest request);
    }
}
