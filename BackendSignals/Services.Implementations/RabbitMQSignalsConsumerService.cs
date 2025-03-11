using System.Text;
using BackendSignals.Requests;
using System.Text.Json;
using BackendSignals.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendSignals.Services.Implementations
{
    public class RabbitMQSignalsConsumerService : BackgroundService
    {
        private readonly IChannel _channel;
        private readonly string _queueName;
        private readonly IMeasurementsService _measurementService;

        public RabbitMQSignalsConsumerService(IChannel channel, string queueName, IMeasurementsService measurementService)
        {
            _channel = channel;
            _queueName = queueName;
            _measurementService = measurementService;
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
                    MeasurementRequest measurementRequest = JsonSerializer.Deserialize<MeasurementRequest>(message);
                    await _measurementService.AddMeasurement(measurementRequest);
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message from {_queueName}: {ex}");
                }
            };

            await _channel.BasicConsumeAsync(queue: _queueName,
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
