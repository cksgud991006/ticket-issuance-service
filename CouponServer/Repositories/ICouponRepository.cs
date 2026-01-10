using CouponServer.Domain.Coupons;

namespace CouponServer.Repositories;

public interface ICouponRepository
{
    public Task<bool> HasUserReceivedCoupon(int userId);

    public Task<Coupon?> TryIssueCoupon(int userId);
}