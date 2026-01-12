using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<ClientEntity> Clients => Set<ClientEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderPositionEntity> OrderPositions => Set<OrderPositionEntity>();
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // важливо

        modelBuilder.Entity<OrderPositionEntity>(entity =>
        {
            entity.ToTable("Orders_Positions");
            entity.HasKey(x => x.Id);

            entity.HasOne<OrderEntity>()
                .WithMany(o => o.Positions)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<ProductEntity>(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(x => x.Id);

            entity.HasOne<ClientEntity>(x=>x.Client) 
                .WithMany(o => o.Orders)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<ClientEntity>().HasKey(x => x.Id);

        modelBuilder.Entity<ProductEntity>().HasKey(x => x.Id);
 
    }
}