using BackendSignals.Models;
using BackendSignals.Requests;
using BackendSignals.Responses;
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

        public async Task<List<FlightPositionResponse>> GetFlights()
        {
            var flights = await _flights.Find(flight => true).ToListAsync();
            return flights.Select(flight => new FlightPositionResponse(flight)).ToList();
        }

        public async Task<Flight> CreateFlight(FlightBeginRequest request)
        {
            var flight = new Flight(request);
            await _flights.InsertOneAsync(flight);
            return flight;
        }

        public async Task<FlightResponse> GetFlightByFlightID(long flightID)
        {
            var flight = await _flights.Find<Flight>(flight => flight.FlightID == flightID).FirstOrDefaultAsync();
            return flight != null ? new FlightResponse(flight) : null;
        }

        public async Task<Flight> EndFlight(FlightEndRequest request)
        {
            var update = Builders<Flight>.Update.Set(f => f.EndTime, request.EndTime);
            var options = new FindOneAndUpdateOptions<Flight>
            {
                ReturnDocument = ReturnDocument.After
            };
            var updatedFlight = await _flights.FindOneAndUpdateAsync(
                flight => flight.FlightID == request.FlightID,
                update,
                options);
            return updatedFlight;
        }

        public async Task<List<FlightPositionResponse>> GetFlightPositions()
        {
            var flights = await _flights.Find(f => f.EndTime == null).ToListAsync();
            var responseList = new List<FlightPositionResponse>();

            foreach (var flight in flights)
            {
                if (flight.Measurements != null && flight.Measurements.Any())
                {
                    var latestMeasurement = flight.Measurements
                                                  .OrderByDescending(m => m.Timestamp)
                                                  .First();
                    flight.X = latestMeasurement.X;
                    flight.Y = latestMeasurement.Y;
                    flight.Z = latestMeasurement.Z;
                }

                var dto = new FlightPositionResponse
                {
                    FlightID = flight.FlightID.ToString(),
                    OperatorID = flight.OperatorID,
                    TeamID = flight.TeamID,
                    PlatoonID = flight.PlatoonID,
                    X = flight.X,
                    Y = flight.Y,
                    Z = flight.Z,
                    BeginTime = flight.BeginTime,
                    EndTime = flight.EndTime,
                    Comment = flight.Comment,
                    VideoStream = flight.VideoStream
                };

                responseList.Add(dto);
            }

            return responseList;
        }
    }
}
