using BackendSignals.Services;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.HostName = "localhost";
factory.ClientProvidedName = "app:audit component:event-consumer";

IConnection conn = await factory.CreateConnectionAsync();
IChannel channel = await conn.CreateChannelAsync();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string SIGNALS_QUEUE = "Signals";
const string FLIGHTS_QUEUE = "Flights";

builder.Services.AddHostedService<RabbitMQSignalsConsumerService>(sp =>
{
    return new RabbitMQSignalsConsumerService(channel, SIGNALS_QUEUE);
});

builder.Services.AddHostedService<RabbitMQFlightsConsumerService>(sp =>
{
    return new RabbitMQFlightsConsumerService(channel, FLIGHTS_QUEUE);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
