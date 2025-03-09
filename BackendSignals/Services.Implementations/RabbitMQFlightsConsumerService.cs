using System.Text;
using BackendSignals.Requests;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BackendSignals.Services.Interfaces;

namespace BackendSignals.Services.Implementations
{
    public class RabbitMQFlightsConsumerService : BackgroundService
    {
        private readonly IChannel _channel;
        private readonly string _queueName;
        private readonly IFlightsService _flightsService;

        public RabbitMQFlightsConsumerService(IChannel channel, string queueName, IFlightsService flightsService)
        {
            _channel = channel;
            _queueName = queueName;
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
                    Console.WriteLine($"Queue {_queueName}: {message}");
                    FlightBeginRequest flightRequest = JsonSerializer.Deserialize<FlightBeginRequest>(message);
                    _flightsService.CreateFlight(flightRequest);
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message from {_queueName}: {ex}");
                }
            };

            // Start consuming messages (autoAck set to true)
            await _channel.BasicConsumeAsync(queue: _queueName,
                                             autoAck: true,
                                             consumer: consumer);

            // Keep the service alive until cancellation is requested.
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
