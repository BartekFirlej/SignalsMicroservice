using BackendSignals.Models;
using BackendSignals.Requests;
using BackendSignals.Services.Interfaces;
using MongoDB.Driver;

namespace BackendSignals.Services.Implementations
{
    public class FlightsService : IFlightsService
    {
        private readonly IMongoCollection<Flight> _flights;

        public FlightsService(IMongoClient client)
        {
            var database = client.GetDatabase("Flights");
            _flights = database.GetCollection<Flight>("Flights");
        }

        public async Task<List<Flight>> GetFlights()
        {
            return await _flights.Find(flight => true).ToListAsync();
        }

        public async Task<Flight> CreateFlight(FlightBeginRequest request)
        {
            var flight = new Flight(request);
            await _flights.InsertOneAsync(flight);
            return flight;
        }

        public async Task<Flight> GetFlightByFlightID(long flightID)
        {
            var flight = await _flights.Find<Flight>(flight => long.Parse(flight.FlightID) == flightID).FirstOrDefaultAsync();
            return flight;
        }
    }
}
