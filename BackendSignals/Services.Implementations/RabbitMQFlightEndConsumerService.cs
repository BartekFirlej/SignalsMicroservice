using System.Text;
using BackendSignals.Requests;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BackendSignals.Services.Interfaces;

namespace BackendSignals.Services.Implementations
{
    public class RabbitMQFlightEndConsumerService : BackgroundService
    {
        private readonly IChannel _channel;
        private readonly string _SIGNALS_FLIGHT_END_QUEUE;
        private readonly IFlightsService _flightsService;

        public RabbitMQFlightEndConsumerService(IChannel channel, string SIGNALS_FLIGHT_END_QUEUE, IFlightsService flightsService)
        {
            _channel = channel;
            _SIGNALS_FLIGHT_END_QUEUE = SIGNALS_FLIGHT_END_QUEUE;
            _flightsService = flightsService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"Queue {_SIGNALS_FLIGHT_END_QUEUE}: {message}");
                    FlightEndRequest flightRequest = JsonSerializer.Deserialize<FlightEndRequest>(message);
                    await _flightsService.EndFlight(flightRequest);
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message from {_SIGNALS_FLIGHT_END_QUEUE}: {ex}");
                }
            };

            await _channel.BasicConsumeAsync(queue: _SIGNALS_FLIGHT_END_QUEUE,
                                             autoAck: true,
                                             consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }


        public override void Dispose()
        {
            _channel.CloseAsync();
            base.Dispose();
        }
    }
}
