using Microsoft.EntityFrameworkCore;

namespace Auctions.Models
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Bid> Bid { get; set; }
    }
}