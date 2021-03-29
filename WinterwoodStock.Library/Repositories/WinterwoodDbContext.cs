using Microsoft.EntityFrameworkCore;
using WinterwoodStock.Library.Entities;

namespace WinterwoodStock.Library.Repositories
{
    public class WinterwoodDbContext : DbContext
    {
        public WinterwoodDbContext(DbContextOptions<WinterwoodDbContext> options) : base(options)
        {

        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
