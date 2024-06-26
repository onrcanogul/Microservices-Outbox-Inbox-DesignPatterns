namespace Stock.API.Entities
{
    public class OrderInbox
    {
        public Guid Id { get; set; }
        public bool Processed { get; set; }
        public string Payload { get; set; }
    }
}
