using Microsoft.Extensions.Configuration;
using NetCoreRabbitMQ.Domain.ValueObjects;
using RabbitMQ.Client;
using MemoryPack;
using RabbitMQ.Client.Events;
using System.Text;


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
            _factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };
            Init().Wait();
        }

        private async Task Init()
        {
            using (var connection = await _factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {
                    await channel.ExchangeDeclareAsync(exchange: BrokerExchanges.OrderExchange, type: ExchangeType.Direct, durable: false);
                }
            }
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            return MemoryPackSerializer.Serialize(obj);
        }

        public async void Publish<T>(T message, string exchangeName, string routingKey)
        {
            using (var connection = await _factory.CreateConnectionAsync())
            {
                using (var channel = await connection.CreateChannelAsync())
                {
                    string text = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(text);

                    await channel.QueueDeclareAsync(queue: routingKey, durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    await channel.QueueBindAsync(queue: routingKey, exchange: BrokerExchanges.OrderExchange, routingKey: routingKey);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" [x] Received {message}");
                        return Task.CompletedTask;
                    };
                    await channel.BasicConsumeAsync(routingKey, autoAck: true, consumer: consumer);
                    await channel.BasicPublishAsync(exchange: BrokerExchanges.OrderExchange, routingKey: routingKey, body: body);
                    await Task.Delay(10000);
                }
            }
        }

        public void Subscribe<T>(string exchangeName, string exchangeType, string queueName, string routingKey, Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}