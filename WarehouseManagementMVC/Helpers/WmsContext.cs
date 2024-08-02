using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Data
{
    public class WmsContext : DbContext
    {
        public WmsContext(DbContextOptions<WmsContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);
            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)           // An Order has one Customer
                .WithMany()           // A Customer can have many Orders
                .HasForeignKey(o => o.CustomerId)  // The foreign key in Order table is CustomerId
                .OnDelete(DeleteBehavior.Cascade);

            // Configure any additional constraints or settings as needed
        }
    }
}