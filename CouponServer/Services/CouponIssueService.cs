using CouponServer.Repositories;
using CouponServer.Domain.Coupons;

namespace CouponServer.Services;

public class CouponIssueService: ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponIssueService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<CouponIssueResult> IssueAsync(
        int userId)
    {

        if (await _couponRepository.HasUserReceivedCoupon(userId))
        {
            return CouponIssueResult.AlreadyIssued();
        }

        var coupon = await _couponRepository.TryIssueCoupon(userId); 
        if (coupon == null)
        {
            return CouponIssueResult.SoldOut();
        }

        return CouponIssueResult.Success(coupon);
    }
}