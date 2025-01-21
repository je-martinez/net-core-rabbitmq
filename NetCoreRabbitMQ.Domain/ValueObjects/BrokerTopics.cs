namespace NetCoreRabbitMQ.Domain.ValueObjects
{
    public static class BrokerExchanges
    {
        public const string OrderExchange = "orders";
    }
    public static class BrokerTopics
    {
        public const string OrderCreated = "orders.created";
    }
}