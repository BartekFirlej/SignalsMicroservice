using BackendSignals;
using BackendSignals.Services.Implementations;
using BackendSignals.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
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

builder.Services.AddSingleton<IFlightsService, FlightsService>();
builder.Services.AddSingleton<IMeasurementsService, MeasurementsService>();

builder.Services.AddHostedService<RabbitMQSignalsConsumerService>(sp =>
{
    var measurementsService = sp.GetRequiredService<IMeasurementsService>();
    return new RabbitMQSignalsConsumerService(channel, SIGNALS_QUEUE, measurementsService);
});

builder.Services.AddHostedService<RabbitMQFlightsConsumerService>(sp =>
{
    var flightsService = sp.GetRequiredService<IFlightsService>();
    return new RabbitMQFlightsConsumerService(channel, FLIGHTS_QUEUE, flightsService);
});

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
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
