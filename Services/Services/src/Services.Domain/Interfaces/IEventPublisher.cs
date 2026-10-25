namespace Clinic.Services.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T eventMessage,
        CancellationToken cancellationToken = default)
        where T : class;
}
