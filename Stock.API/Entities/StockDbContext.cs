using Microsoft.EntityFrameworkCore;

namespace Stock.API.Entities
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<OrderInbox> OrderInboxes { get; set; }

    }
}
