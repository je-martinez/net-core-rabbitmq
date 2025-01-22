using Microsoft.Extensions.Configuration;
using NetCoreRabbitMQ.Domain.ValueObjects;
using RabbitMQ.Client;
using MemoryPack;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using static NetCoreRabbitMQ.Domain.ValueObjects.BrokerExchanges;


namespace NetCoreRabbitMQ.Application.Providers
{

    [MemoryPackable]
    public partial class PublishableMessage
    {
        public string Type { get; set; }
    }

    public class BrokerProvider : IBrokerProvider
    {

        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        public BrokerProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            /* This code snippet is initializing a `ConnectionFactory` object with specific configuration
            settings for connecting to a RabbitMQ server. Here's a breakdown of what each property is being
            set to: */
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
                VirtualHost = _configuration["RabbitMQ:VirtualHost"]
            };
        }

        private async Task Init()
        {
            using (var connection = await _factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {
                    foreach (var exchange in AllAvailableExchanges)
                    {
                        await channel.ExchangeDeclareAsync(exchange: exchange.Value.Name, type: ExchangeType.Topic, durable: false);
                        foreach (var queue in exchange.Value.Queue)
                        {
                            await channel.QueueDeclareAsync(queue: queue.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                            await channel.QueueBindAsync(queue: queue.Queue, exchange: exchange.Value.Name, routingKey: queue.RoutingKey);
                        }
                    }
                }
            }
        }

        public static byte[] ObjectToByteArray<T>(T obj)
        {
            byte[] messageBytes = JsonSerializer.SerializeToUtf8Bytes(obj);
            return messageBytes;
        }

        public async void Publish<T>(T message, ExchangeAppTypes type, SupportedBrokerRoutingKeys key)
        {
            using (var connection = await _factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {

                    await Init();

                    BrokerExchange? exchange = GetExchangeName(type);

                    if (exchange == null)
                    {
                        return;
                    }

                    string? routingKey = BrokerRoutingKeys.GetRoutingKey(key);

                    if (string.IsNullOrEmpty(routingKey))
                    {
                        return;
                    }

                    await channel.QueueDeclareAsync(queue: routingKey, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(queue: routingKey, exchange.Name, routingKey: routingKey);
                    await channel.BasicPublishAsync(exchange: exchange.Name, routingKey: routingKey, body: ObjectToByteArray(message));
                }
            }
        }


        public async void Publish<T>(T message, ExchangeAppTypes type, string routingKey)
        {
            using (var connection = await _factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {

                    await Init();

                    BrokerExchange? exchange = GetExchangeName(type);

                    if (exchange == null)
                    {
                        return;
                    }

                    await channel.BasicPublishAsync(exchange: exchange.Name, routingKey: routingKey, body: ObjectToByteArray(message));
                    await Task.Delay(10000);
                }
            }
        }
    }
}