using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BackendSignals.Services
{
    public class RabbitMQSignalsConsumerService : BackgroundService
    {
        private readonly IChannel _channel;
        private readonly string _queueName;

        public RabbitMQSignalsConsumerService(IChannel channel, string queueName)
        {
            _channel = channel;
            _queueName = queueName;
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

                    // TODO: Add your asynchronous message processing logic here.
                    await Task.Delay(1000);
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
