namespace Clinic.Profile.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T eventMessage, CancellationToken cancellationToken = default)
        where T : class;
}