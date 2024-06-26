namespace Order.API.Models
{
    public class OrderOutbox
    {
        public Guid Id { get; set; }
        public DateTime OccuredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
    }
}
