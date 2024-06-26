namespace Order.API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
