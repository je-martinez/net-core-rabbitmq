using NetCoreRabbitMQ.Domain.Common;

namespace NetCoreRabbitMQ.Domain.Entities
{
    public class Session : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}