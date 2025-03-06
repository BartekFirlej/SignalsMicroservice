using BackendSignals.Models;
using BackendSignals.Requests;
using BackendSignals.Responses;

namespace BackendSignals.Services.Interfaces
{
    public interface IFlightsService
    {
        public Task<List<Flight>> GetFlights();
        public Task<Flight> GetFlightByFlightID(long flightID);
        public Task<Flight> CreateFlight(FlightBeginRequest request);
    }
}
