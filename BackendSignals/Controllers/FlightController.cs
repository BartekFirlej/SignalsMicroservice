using BackendSignals.Requests;
using BackendSignals.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendSignals.Controllers
{
    [ApiController]
    [Route("flights")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightsService _flightService;

        public FlightController(IFlightsService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var flights = await _flightService.GetFlights();
            return Ok(flights);
        }

        [HttpGet("flightID")]
        public async Task<IActionResult> GetFlightByID(long flightID)
        {
            var flight = await _flightService.GetFlightByFlightID(flightID);
            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> PostFlight(FlightBeginRequest request)
        {
            var flight = await _flightService.CreateFlight(request);
            return CreatedAtAction(nameof(PostFlight), flight);
        }
    }
}
