using NetCoreRabbitMQ.Domain.Common;

namespace NetCoreRabbitMQ.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}