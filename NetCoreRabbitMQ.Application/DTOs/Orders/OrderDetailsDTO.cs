namespace NetCoreRabbitMQ.Application.DTOs.Orders
{
    public class OrderDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; } = 0;
    }
}