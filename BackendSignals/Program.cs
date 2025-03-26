using BackendSignals;
using BackendSignals.Services.Implementations;
using BackendSignals.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

ConnectionFactory factory = new ConnectionFactory();
factory.UserName = configuration["RabbitMQ:UserName"];
factory.Password = configuration["RabbitMQ:Password"];
factory.HostName = configuration["RabbitMQ:HostName"];
factory.ClientProvidedName = configuration["RabbitMQ:ClientProvidedName"];

var queuesSection = builder.Configuration.GetSection("RabbitMQ:Queues");

string SIGNALS_FLIGHT_BEGIN_QUEUE = queuesSection["SignalsFlightBegin"];
string SIGNALS_FLIGHT_END_QUEUE = queuesSection["SignalsFlightEnd"];
string SIGNALS_SIGNALS_QUEUE = queuesSection["SignalsSignals"];

IConnection conn = await factory.CreateConnectionAsync();
IChannel channel = await conn.CreateChannelAsync();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IFlightsService, FlightsService>();
builder.Services.AddSingleton<IMeasurementsService, MeasurementsService>();

builder.Services.AddHostedService<RabbitMQFlightBeginConsumerService>(sp =>
{
    var flightsService = sp.GetRequiredService<IFlightsService>();
    return new RabbitMQFlightBeginConsumerService(channel, SIGNALS_FLIGHT_BEGIN_QUEUE, flightsService);
});

builder.Services.AddHostedService<RabbitMQSignalsConsumerService>(sp =>
{
    var measurementsService = sp.GetRequiredService<IMeasurementsService>();
    return new RabbitMQSignalsConsumerService(channel, SIGNALS_SIGNALS_QUEUE, measurementsService);
});

builder.Services.AddHostedService<RabbitMQFlightEndConsumerService>(sp =>
{
    var flightsService = sp.GetRequiredService<IFlightsService>();
    return new RabbitMQFlightEndConsumerService(channel, SIGNALS_FLIGHT_END_QUEUE, flightsService);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
