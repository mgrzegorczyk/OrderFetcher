using Microsoft.EntityFrameworkCore;
using OrderFetcher.Domain.Entities;

namespace OrderFetcher.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}