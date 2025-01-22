using NetCoreRabbitMQ.Domain.ValueObjects;

namespace NetCoreRabbitMQ.Application.Providers
{
    public interface IBrokerProvider
    {
        void Publish<T>(T message, ExchangeAppTypes type, SupportedBrokerRoutingKeys routingKey);
        void Publish<T>(T message, ExchangeAppTypes type, string routingKey);
        void Subscribe<T>(ExchangeAppTypes exchangeName, SupportedBrokerRoutingKeys routingKey, Action<T> action);
    }
}