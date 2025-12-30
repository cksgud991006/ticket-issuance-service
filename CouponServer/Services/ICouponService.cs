using CouponServer.Repositories;
using CouponServer.Domain.Coupons;

namespace CouponServer.Services;

public interface ICouponService
{
    public Task<CouponIssueResult> IssueAsync(
        int userId);
}