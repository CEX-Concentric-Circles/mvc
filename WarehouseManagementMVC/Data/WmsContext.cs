using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Models;

public class WmsContext : DbContext
{
    public WmsContext(DbContextOptions<WmsContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}