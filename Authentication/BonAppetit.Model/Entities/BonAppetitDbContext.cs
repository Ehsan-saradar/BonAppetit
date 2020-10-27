using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BonAppetit.Model.Entities
{
    public class BonAppetitDbContext : IdentityDbContext<CustomerUser>
    {
        public BonAppetitDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<RestaurantUser> RestaurantUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        public DbSet<RestaurantRate> RestaurantRates { get; set; }
    }
}