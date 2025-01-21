namespace NetCoreRabbitMQ.Application.Providers
{
    public interface IBrokerProvider
    {
        void Publish<T>(T message, string exchangeName, string routingKey);
        void Subscribe<T>(string exchangeName, string exchangeType, string queueName, string routingKey, Action<T> action);
    }
}