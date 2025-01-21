namespace NetCoreRabbitMQ.Application.DTOs.Orders
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public string TrackingNumber { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Guid SessionId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }
}