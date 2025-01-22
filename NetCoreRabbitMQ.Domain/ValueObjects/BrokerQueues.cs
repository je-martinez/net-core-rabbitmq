namespace NetCoreRabbitMQ.Domain.ValueObjects
{
    public static class BrokerQueues
    {
        public const string OrdersCreated = "*.orders.order-created";
        public const string OrderStatusChange = "*.orders.orders-status-changed";
        public const string Notifications = "*.notifications";
    }
}