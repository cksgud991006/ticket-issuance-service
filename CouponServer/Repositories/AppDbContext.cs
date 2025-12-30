using Microsoft.EntityFrameworkCore;
using CouponServer.Domain.Coupons;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Net.Http.Headers;

namespace CouponServer.Repositories;
public class AppDbContext: DbContext
{

    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CouponPolicy> CouponPolicy { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(c => c.CouponId);

            entity.Property(c => c.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<CouponPolicy>(entity =>
        {
            entity.HasIndex(cp => cp.Id)
                .IsUnique();

            entity.Property(cp => cp.TotalQuantity)
                .IsRequired()
                .HasDefaultValue(1000);

            entity.Property(cp => cp.IssuedCoupons)
                .IsRequired()
                .HasDefaultValue(0);
        });
    }
}