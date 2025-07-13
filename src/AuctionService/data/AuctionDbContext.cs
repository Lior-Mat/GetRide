
using Microsoft.EntityFrameworkCore;
namespace AuctionService.data;


public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Modules.Auction> Auctions { get; set; } = null!;
}

