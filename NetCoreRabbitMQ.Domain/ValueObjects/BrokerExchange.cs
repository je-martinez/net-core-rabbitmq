namespace NetCoreRabbitMQ.Domain.ValueObjects
{
    public enum ExchangeAppTypes
    {
        Orders,
        Notifications
    }

    public class BrokerExchange
    {
        public string Name { get; set; }
        public ExchangeAppTypes Type { get; set; }
        public List<BrokerQueueRoutingKey> Queue { get; set; }
    }


    public static class BrokerExchanges
    {
        public const string OrdersExchange = "orders";
        public const string NotificationsExchange = "notifications";
        public static readonly Dictionary<ExchangeAppTypes, BrokerExchange> AllAvailableExchanges = new Dictionary<ExchangeAppTypes, BrokerExchange>
        {
            {
                ExchangeAppTypes.Orders,
                new BrokerExchange
                {
                    Name = OrdersExchange,
                    Type = ExchangeAppTypes.Orders,
                    Queue = new List<BrokerQueueRoutingKey>
                    {
                        new BrokerQueueRoutingKey { Queue = BrokerQueues.OrdersCreated, RoutingKey = BrokerRoutingKeys.OrdersCreated },
                        new BrokerQueueRoutingKey { Queue = BrokerQueues.OrderStatusChange, RoutingKey = BrokerRoutingKeys.OrderStatusChange }
                    }
                }
            },
            {
                ExchangeAppTypes.Notifications,
                new BrokerExchange
                {
                    Name = NotificationsExchange,
                    Type = ExchangeAppTypes.Notifications,
                    Queue = new List<BrokerQueueRoutingKey>
                    {
                        new BrokerQueueRoutingKey { Queue = BrokerQueues.Notifications, RoutingKey = BrokerRoutingKeys.Notifications }
                    }
                }
            }
        };

        public static BrokerExchange? GetExchangeName(ExchangeAppTypes exchangeAppType)
        {
            return AllAvailableExchanges.TryGetValue(exchangeAppType, out var exchange) ? exchange : null;
        }
    }
}