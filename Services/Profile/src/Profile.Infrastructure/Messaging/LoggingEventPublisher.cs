using System.Text.Json;
using Clinic.Profile.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clinic.Profile.Infrastructure.Messaging;

public class LoggingEventPublisher : IEventPublisher
{
    private readonly ILogger<LoggingEventPublisher> _logger;
    
    public LoggingEventPublisher(ILogger<LoggingEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<T>(
        T eventMessage,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var json = JsonSerializer.Serialize(eventMessage);
        _logger.LogInformation(
            "EVENT STUB [{EventType}]: {Payload}",
            typeof(T).Name,
            json);

        return Task.CompletedTask;
    }
}