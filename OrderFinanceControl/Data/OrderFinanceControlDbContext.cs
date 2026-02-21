using Microsoft.EntityFrameworkCore;
using OrderFinanceControl.Entities;
namespace OrderFinanceControl.Data;

public class OrderFinanceControlDbContext : DbContext
{
    public OrderFinanceControlDbContext(DbContextOptions<OrderFinanceControlDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ========================
        // CUSTOMER
        // ========================
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasIndex(c => c.Email)
                .IsUnique();
        });

        // ========================
        // PRODUCT
        // ========================
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        // ========================
        // ORDER
        // ========================
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(o => o.Id);

            entity.Property(o => o.CreatedAt)
                .IsRequired();

            entity.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(o => o.Status)
                .HasConversion<string>() 
                .IsRequired();

            entity.Property(o => o.PaidAt);

            entity.HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ========================
        // ORDER ITEM
        // ========================
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");

            entity.HasKey(i => i.Id);

            entity.Property(i => i.Quantity)
                .IsRequired();

            entity.Property(i => i.UnitPriceAtOrderTime)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}