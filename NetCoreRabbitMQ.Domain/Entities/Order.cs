using NetCoreRabbitMQ.Domain.Common;

namespace NetCoreRabbitMQ.Domain.Entities
{

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Failed,
    }

    public class Order : BaseEntity
    {
        public required string OrderNumber { get; set; } = Guid.NewGuid().ToString();
        public required string TrackingNumber { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset OrderDate { get; set; } = DateTime.Now.ToUniversalTime();
        public Guid SessionId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public virtual Session Session { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}