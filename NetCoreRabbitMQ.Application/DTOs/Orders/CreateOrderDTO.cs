namespace NetCoreRabbitMQ.Application.DTOs.Orders
{
    public class CreateOrderDTO
    {
        public Guid SessionId { get; set; }
        public List<CreateOrderDetailsDTO> OrderDetails { get; set; }
    }

    public class CreateOrderDetailsDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}