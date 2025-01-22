using System.Text;
using NetCoreRabbitMQ.Domain.ValueObjects;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetCoreRabbitMQ.OrdersWorker.Background;

public class OrdersWorker : BackgroundService
{
    private readonly ILogger<OrdersWorker> _logger;
    private readonly IConfiguration _configuration;
    private readonly ConnectionFactory _factory;

    private IConnection _connection;

    private IChannel _channel;

    public OrdersWorker(ILogger<OrdersWorker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"],
            UserName = _configuration["RabbitMQ:UserName"],
            Password = _configuration["RabbitMQ:Password"],
            VirtualHost = _configuration["RabbitMQ:VirtualHost"]
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        BrokerExchange exchange = BrokerExchanges.GetExchangeName(ExchangeAppTypes.Orders)!;
        await RegisterExchangeAndQueues(exchange, _channel);
        await RegisterOrderCreatedConsumer(_channel);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Dispose();
        _connection?.Dispose();
        _logger.LogInformation("Worker stopped.");
        return base.StopAsync(cancellationToken);
    }

    private async Task RegisterExchangeAndQueues(BrokerExchange exchange, IChannel channel)
    {
        await channel.ExchangeDeclareAsync(exchange: exchange.Name, type: ExchangeType.Topic, durable: false);
        var operations = exchange.Queue.Select(async queue =>
        {
            await channel.QueueDeclareAsync(queue: queue.Queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queue: queue.Queue, exchange: exchange.Name, routingKey: queue.RoutingKey);
        });
        await Task.WhenAll(operations);
        _logger.LogInformation("Exchange and queues registered.");
    }

    private async Task RegisterOrderCreatedConsumer(IChannel channel)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation(message);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(BrokerRoutingKeys.GetRoutingKey(SupportedBrokerRoutingKeys.OrdersCreated)!, autoAck: true, consumer: consumer);
        _logger.LogInformation("OrdersWorker started. Ready to receive messages.");
    }
}
