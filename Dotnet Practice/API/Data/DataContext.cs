using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Carts> Carts { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}