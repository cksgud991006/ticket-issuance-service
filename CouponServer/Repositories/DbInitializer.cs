using Microsoft.EntityFrameworkCore;
using CouponServer.Domain.Coupons;

namespace CouponServer.Repositories;

public static class DbInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate();

        if (!context.CouponPolicy.Any())
        {
            context.CouponPolicy.Add(CouponPolicy.CreateDefault());
            context.SaveChanges();
        }

        else
        {
            context.CouponPolicy.ExecuteUpdate(p =>
                p.SetProperty(
                    x => x.IssuedCoupons,
                    x => context.Coupons.Count()
                )
            );
        }
    }
}