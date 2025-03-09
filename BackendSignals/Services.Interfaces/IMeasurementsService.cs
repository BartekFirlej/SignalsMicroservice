using BackendSignals.Models;
using BackendSignals.Requests;

namespace BackendSignals.Services.Interfaces
{
    public interface IMeasurementsService
    {
        public Task<Measurement> AddMeasurement(MeasurementRequest request);
    }
}
