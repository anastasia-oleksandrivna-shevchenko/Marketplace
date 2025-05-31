using Marketplace.Entities;

namespace Marketplace.Data;

using Microsoft.EntityFrameworkCore;

public class MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Review> Review { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);

            entity.Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.MiddleName)
                .HasMaxLength(50);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Phone)
                .IsRequired();

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(200); 

            entity.Property(u => u.Role)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Buyer', 'Seller')"));
        });
        
        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(s => s.StoreId);

            entity.Property(s => s.StoreId)
                .ValueGeneratedOnAdd();

            entity.Property(s => s.UserId)
                .IsRequired();

            entity.HasOne(s => s.User)
                .WithMany(u => u.Stores)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.Description)
                .HasMaxLength(1000);

            entity.Property(s => s.Location)
                .HasMaxLength(200);

            entity.Property(s => s.Rating)
                .IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_Store_Rating", "[Rating] >= 0 AND [Rating] <= 5"));
        });
        
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductId);

            entity.Property(p => p.ProductId)
                .ValueGeneratedOnAdd();

            entity.Property(p => p.StoreId)
                .IsRequired();

            entity.Property(p => p.CategoryId)
                .IsRequired();

            entity.HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(11,2)")
                .IsRequired();

            entity.Property(p => p.Quantity)
                .IsRequired();

            entity.Property(p => p.ImageUrl)
                .HasMaxLength(500);
        });

        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.OrderId);

            entity.Property(o => o.OrderId)
                .ValueGeneratedOnAdd();

            entity.Property(o => o.CustomerId)
                .IsRequired();

            entity.Property(o => o.StoreId)
                .IsRequired();

            entity.Property(o => o.OrderDate)
                .IsRequired();

            entity.Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(11,2)");

            entity.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(20);
            
            entity.HasCheckConstraint("CK_Order_Status", "[Status] IN ('Pending', 'Shipped', 'Completed')");

            entity.HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StoreId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.OrderItemId);

            entity.Property(oi => oi.OrderItemId)
                .ValueGeneratedOnAdd();

            entity.Property(oi => oi.OrderId)
                .IsRequired();

            entity.Property(oi => oi.ProductId)
                .IsRequired();

            entity.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(oi => oi.Quantity)
                .IsRequired();

            entity.Property(oi => oi.Price)
                .IsRequired()
                .HasColumnType("decimal(11,2)");
        });

        
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.ReviewId);

            entity.Property(r => r.ReviewId)
                .ValueGeneratedOnAdd();

            entity.Property(r => r.ProductId)
                .IsRequired();

            entity.Property(r => r.UserId)
                .IsRequired();

            entity.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(r => r.Rating)
                .IsRequired();

            entity.Property(r => r.Comment)
                .HasMaxLength(1000);

            entity.Property(r => r.CreatedAt)
                .IsRequired();

            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5"));
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.CategoryId);

            entity.Property(c => c.CategoryId)
                .ValueGeneratedOnAdd();

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            
        });
    }
}