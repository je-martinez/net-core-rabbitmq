namespace NetCoreRabbitMQ.Domain.ValueObjects
{

    public class BrokerQueueRoutingKey
    {
        public string Queue { get; set; }
        public string RoutingKey { get; set; }
    }


    public enum SupportedBrokerRoutingKeys
    {
        OrdersCreated,
        OrderStatusChange,
        Notifications
    }

    public static class BrokerRoutingKeys
    {
        public const string OrdersCreated = "*.orders.order-created";
        public const string OrderStatusChange = "*.orders.orders-status-changed";
        public const string Notifications = "*.notifications";
        public static Dictionary<SupportedBrokerRoutingKeys, string> SupportedRoutingKeys = new Dictionary<SupportedBrokerRoutingKeys, string>
        {
            { SupportedBrokerRoutingKeys.OrdersCreated, OrdersCreated },
            { SupportedBrokerRoutingKeys.OrderStatusChange, OrderStatusChange },
            { SupportedBrokerRoutingKeys.Notifications, Notifications }
        };


        public static string? GetRoutingKey(SupportedBrokerRoutingKeys routingKey)
        {
            return SupportedRoutingKeys.TryGetValue(routingKey, out var supportedKey) ? supportedKey : null;
        }
    }
}