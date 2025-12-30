using Microsoft.EntityFrameworkCore;
using CouponServer.Domain.Coupons;
using CouponServer.Repositories;

namespace CouponServer.Tests.Fakes;

public class FakeCouponRepository : ICouponRepository
{
    private readonly List<Coupon> _coupons = new();

    private int _totalQuantity;
    private int _issuedCoupons;

    public FakeCouponRepository()
    {
        _coupons = new List<Coupon>();
        _totalQuantity = 1;
        _issuedCoupons = 0;
    }

    public FakeCouponRepository(int totalQuantity)
    {
        _coupons = new List<Coupon>();
        _totalQuantity = totalQuantity;
        _issuedCoupons = 0;
    }

    public Task<bool> HasUserReceivedCoupon(int userId)
    {
        return Task.FromResult(
            _coupons.Any(c => c.UserId == userId)
        );
    }


    public Task<bool> TryIssueCoupon(int userId)
    {

        // mirrors DB constraint: inventory exhausted
        if (_issuedCoupons >= _totalQuantity)
            return Task.FromResult(false);

        var coupon = Coupon.Create(userId);
        _coupons.Add(coupon);
        _issuedCoupons++;
        
        return Task.FromResult(true);
    }
}