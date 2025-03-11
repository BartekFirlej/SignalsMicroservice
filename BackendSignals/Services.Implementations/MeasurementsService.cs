using BackendSignals.Models;
using BackendSignals.Requests;
using BackendSignals.Services.Interfaces;
using MongoDB.Driver;

namespace BackendSignals.Services.Implementations
{
    public class MeasurementsService : IMeasurementsService
    {
        private readonly IMongoCollection<Flight> _flights;

        public MeasurementsService(IMongoClient client)
        {
            var database = client.GetDatabase("Flights");
            _flights = database.GetCollection<Flight>("Flights");
        }

        public async Task<Measurement> AddMeasurement(MeasurementRequest request)
        {
            var measurement = new Measurement(request);
            var update = Builders<Flight>.Update.Push(f => f.Measurements, measurement);
            await _flights.UpdateOneAsync(flight => flight.FlightID == request.FlightId, update);
            return measurement;
        }
    }
}
